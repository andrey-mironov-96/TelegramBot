using System.Text.RegularExpressions;
using app.common.DTO;
using app.domain.Abstract;
using Microsoft.Extensions.Logging;
using WebParse.Business;
using WebParse.Models;
using WebParse.Utils.Enums;

namespace WebParse.Services
{
    public class WebParseService : IWebParseService
    {
        private readonly IStackTraceService stackTraceService;

        public ILogger<WebParseService> Logger { get; }

        public WebParseService(
            ILogger<WebParseService> logger,
            IStackTraceService stackTraceService)
        {
            Logger = logger;
            this.stackTraceService = stackTraceService;
        }
        public async Task<Dictionary<string, List<AdmissionPlan>>> GetFacultiesAndSpecialitiesAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage getResult = await httpClient.GetAsync("https://abiturient.ulsu.ru/tiles/documents/86");
            string stringResult = await getResult.Content.ReadAsStringAsync();
            stringResult = stringResult.Replace("\\u003c", "<").Replace("\\u003e", ">").Replace("\\n", "").Replace("\\r", "").Replace("\\t", "").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("&nbsp;", "");
            int cutHead = stringResult.IndexOf("</thead><tbody>");
            if (cutHead != -1)
            {
                var dirtString = stringResult.Remove(0, cutHead);
                string[] educationstypes = dirtString.Split("<strong>СРЕДНЕЕ ПРОФЕССИОНАЛЬНОЕ ОБРАЗОВАНИЕ</strong>");
                if (educationstypes.Count() == 2)
                {
                    string higherEducation = educationstypes[0];
                    Regex regexRmTdStyle = new Regex("\\s*style=\\\"height:\\s*\\d*px;\\s*width:\\s*\\d*.?\\d*%;\\\">");
                    higherEducation = regexRmTdStyle.Replace(higherEducation, ">");
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

        public async Task<ParsingResult<Dictionary<string, List<AdmissionPlan>>>> GetPriceForSpecialities(Dictionary<string, List<AdmissionPlan>> facultyies)
        {
            HttpClient httpClient = new HttpClient();
            ParsingResult<Dictionary<string, List<AdmissionPlan>>> parsingResult = new ParsingResult<Dictionary<string, List<AdmissionPlan>>>();
            parsingResult.Data = facultyies;
            HttpResponseMessage getResult = await httpClient.GetAsync("https://abiturient.ulsu.ru/tiles/information/cost");
            string stringResult = await getResult.Content.ReadAsStringAsync();
            string[] dirt = stringResult.Split("type=\"application/json\">", StringSplitOptions.None);
            if (dirt.Count() != 2)
            {
                parsingResult.ResultParse = false;
                parsingResult.StackTraces.Add(new StackTraceDTO()
                {
                    Step = "Шаг 2. Получение стоимости обучения",
                    Error = "Ошибка, дом-дерево документы было изменено! Код: 1",
                    Identity = Guid.NewGuid()
                });
                return parsingResult;
            }
            string pattern = "Очная форма  обучения</td> </tr>";
            int index = dirt[1].IndexOf(pattern);
            string dirtTable = dirt[1].Replace("\\u003c", "<").Replace("\\u003e", ">").Replace("\\n", "").Replace("\\r", "").Replace("\\t", "").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\\u0026nbsp;", "");
            Regex regexRmClass = new Regex("class=\\\\\\\"\\S*\"");
            string prepareTable = regexRmClass.Replace(dirtTable, "");

            Regex regexRmStyle = new Regex("style=\\\\\\\"\\S*\"");
            prepareTable = regexRmStyle.Replace(prepareTable, "");

            Regex regexRmHeight = new Regex("height=\\\\\\\"\\S*\"");
            prepareTable = regexRmHeight.Replace(prepareTable, "");

            Regex regexSpaceTRHeight = new Regex("<tr\\s*>");
            prepareTable = regexSpaceTRHeight.Replace(prepareTable, "<tr>");

            Regex regexSpaceTDHeight = new Regex("<td\\s*>");
            prepareTable = regexSpaceTDHeight.Replace(prepareTable, "<td>");

            Regex regexRMColspan = new Regex("\\s*colspan=\\\\\\\"\\d*\\\\\\\"\\s*>");
            prepareTable = regexRMColspan.Replace(prepareTable, ">");

            string[] rows = prepareTable.Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
            rows = rows.Where(row => row.Trim().Length != 0).ToArray().Where((item, index) => index > 2).Select(row => row.Trim()).ToArray();
            if (rows.Length == 0)
            {
                parsingResult.ResultParse = false;
                parsingResult.StackTraces.Add(new StackTraceDTO()
                {
                    Step = "Шаг 2. Получение стоимости обучения",
                    Error = "Ошибка, дом-дерево документы было изменено! Код:2",
                    Identity = Guid.NewGuid()
                });
                return parsingResult;
            }
            Dictionary<string, long> data = new Dictionary<string, long>();
            TypeEducation typeEducation = TypeEducation.None;
            KeyValuePair<string, List<AdmissionPlan>> faculty = new KeyValuePair<string, List<AdmissionPlan>>();
            string nameFaculty = "";
            Regex regRmSpeces = new Regex("\\s{2}");
            Regex regRmSpecialityCode = new Regex("\\d{2}.\\d{2}.\\d{2}\\s");
            foreach (string row in rows)
            {
                string[] colums = row.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                colums[0] = regRmSpeces.Replace(colums[0], " ");
                if (colums.Length == 1)
                {
                    if (colums[0].ToLower().Contains("очная"))
                    {
                        typeEducation = TypeEducation.FullTime;
                    }
                    else if (colums[0].ToLower().Contains("заочная форма"))
                    {
                        typeEducation = TypeEducation.Distance;
                    }
                    else if (colums[0].ToLower().Contains("очно-заочная"))
                    {
                        typeEducation = TypeEducation.PartTime;
                    }
                    else
                    {
                        nameFaculty = colums[0];
                        faculty = parsingResult.Data.SingleOrDefault(fac => fac.Key == nameFaculty);
                    }
                }
                else if (colums.Length == 3)
                {
                    if (faculty.Key is null)
                    {
                        parsingResult.ResultParse = false;
                        parsingResult.StackTraces.Add(new StackTraceDTO()
                        {
                            Step = "Шаг 2. Получение стоимости обучения",
                            Error = $"Не найден факультет {nameFaculty}",
                            Identity = Guid.NewGuid()
                        });
                        continue;
                    }
                    if (typeEducation == TypeEducation.None)
                    {
                        parsingResult.ResultParse = false;
                        parsingResult.StackTraces.Add(new StackTraceDTO()
                        {
                            Step = "Шаг 2. Получение стоимости обучения",
                            Error = $"Не найден тип обучения для факультета {nameFaculty}",
                            Identity = Guid.NewGuid()
                        });
                        continue;
                    }
                    if (!long.TryParse(colums[2].Replace("<br>", ""), out long price))
                    {
                        parsingResult.ResultParse = false;
                        parsingResult.StackTraces.Add(new StackTraceDTO()
                        {
                            Step = "Шаг 2. Получение стоимости обучения",
                            Error = $"Не смог спарсить стоимость {colums[1]}",
                            Identity = Guid.NewGuid()
                        });
                        continue;
                    }
                    AdmissionPlan speciality = faculty.Value.SingleOrDefault(spec => spec.TypeEducation == typeEducation && spec.SpecialtyName.ToLower() == colums[0].ToLower());
                    if (speciality is null)
                    {
                        parsingResult.ResultParse = false;
                        parsingResult.StackTraces.Add(new StackTraceDTO()
                        {
                            Step = "Шаг 2. Получение стоимости обучения",
                            Error = $"Не найдена специальность \"{colums[0]}\" факультета \"{nameFaculty}\"",
                            Identity = Guid.NewGuid()
                        });
                        continue;
                    }
                    speciality.Price = price;
                }
            }

            return parsingResult;

        }

        public async Task<ParsingResult<Dictionary<string, List<AdmissionPlan>>>> GetDataFromULGUSite()
        {
            Dictionary<string, List<AdmissionPlan>> faculties = await this.GetFacultiesAndSpecialitiesAsync();
            if (faculties == null || faculties.Count == 0)
            {
                throw new ArgumentNullException("Коллекция факультетов и специальностей пуста");
            }
            ParsingResult<Dictionary<string, List<AdmissionPlan>>> parsingResult = await GetPriceForSpecialities(faculties);
            return parsingResult;
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