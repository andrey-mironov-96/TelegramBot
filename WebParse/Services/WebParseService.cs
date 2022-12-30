using WebParse.Business;
using WebParse.Models;
using WebParse.Utils.Enums;

namespace WebParse.Services
{
    public class WebParseService : IWebParseService
    {
        public async Task<Dictionary<string, List<AdmissionPlan>>> GetAsync(string http, HttpType httpType)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage getResult = await httpClient.GetAsync(http);
            string stringResult = await getResult.Content.ReadAsStringAsync();
            stringResult = stringResult.Replace("\\u003c", "<").Replace("\\u003e", ">").Replace("\\n", "").Replace("\\r", "").Replace("\\t", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
            int cutHead = stringResult.IndexOf("</thead><tbody>");
            if (cutHead != -1)
            {
                var dirtString = stringResult.Remove(0, cutHead);
                string[] educationstypes = dirtString.Split("<strong>СРЕДНЕЕ ПРОФЕССИОНАЛЬНОЕ ОБРАЗОВАНИЕ&nbsp;</strong>");
                if (educationstypes.Count() == 2)
                {
                    //Обработка среднего образования
                    string middleEducation = educationstypes[1];
                    int indexPositionStartScripts = middleEducation.IndexOf("<script id=\"__NEXT_DATA__\"");
                    if (indexPositionStartScripts != -1)
                    {
                        middleEducation = middleEducation.Remove(indexPositionStartScripts, middleEducation.Length - indexPositionStartScripts);
                    }
                    //Обработка высшего образования
                    string higherEducation = educationstypes[0];
                    string[] allBlocksHigherEducation = higherEducation.Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (allBlocksHigherEducation.Count() == 0)
                    {
                        throw new ArgumentException("not found blocks");
                    }
                    TypeEducation type = TypeEducation.None;
                    string facultetName = string.Empty;
                    List<AdmissionPlan> admissionPlans = new List<AdmissionPlan>();
                    foreach (string block in allBlocksHigherEducation)
                    {
                        if (block.Contains("fcolor") && block.Contains("Очная форма обучения"))
                        {
                            type = TypeEducation.FullTime;
                        }
                        else if (block.Contains("fcolor") && block.Contains("Очно-заочная форма обучения"))
                        {
                            type = TypeEducation.PartTime;
                        }
                        else if (block.Contains("fcolor") && block.Contains("Заочная форма обучения"))
                        {
                            type = TypeEducation.Distance;
                        }
                        else if (block.Contains("icolor"))
                        {
                            string[] spliting = block.Split(new string[] { "<strong>", "</strong>" }, StringSplitOptions.TrimEntries);
                            if (spliting.Count() != 3)
                            {
                                //TODO not fount name facultet
                            }
                            facultetName = spliting[1];
                        }
                        else
                        {
                            if (block.Contains("<td>"))
                            {
                                string[] spliting = block.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.None);
                                    admissionPlans.Add(new AdmissionPlan
                                    {
                                        SpecialtyName = RemoveDirtInString(spliting[1]),
                                        GeneralCompetition = ParseAdmision(spliting[3]),
                                        QuotaLOP = ParseAdmision(spliting[5]),
                                        TargetAdmissionQuota = ParseAdmision(spliting[7]),
                                        SpecialQuota = ParseAdmision(spliting[9]),
                                        ExtrabudgetaryPlaces = ParseAdmision(spliting[11]),
                                        TypeEducation = type,
                                        FacultetName = RemoveDirtInString(facultetName)
                                    });
                                

                            }
                        }
                    }
                    if (admissionPlans.Count > 0)
                    {
                        return admissionPlans.GroupBy(o => o.FacultetName)
                            .ToDictionary(g => g.Key, g => g.ToList());
                    }
                }
            }
            return null;
        }

        private int ParseAdmision(string value)
        {
            string pureLine = RemoveDirtInString(value);
            if (String.IsNullOrEmpty(pureLine) || String.IsNullOrWhiteSpace(pureLine))
            {
                return 0;
            }
            int resultValue = 0;
            Int32.TryParse(pureLine, out resultValue);
            return resultValue;
        }

        private string RemoveDirtInString(string value)
        {
            string result = value.Replace("&nbsp;", "").Replace("<br>", "").Trim();
            return result;
        }

        private string[] ClearBlock(string[] values)
        {
            if (values.Contains("23.03.02 Наземные транспортно-технологические комплексы"))
            {

            }

            List<string> pureElements = new List<string>();
            for (int i = 0; i < values.Count(); i++)
            {
                if (!String.IsNullOrWhiteSpace(values[i]) && !String.IsNullOrEmpty(values[i]))
                {
                    pureElements.Add(values[i]);
                }
            }
            return pureElements.ToArray();

        }
    }

}