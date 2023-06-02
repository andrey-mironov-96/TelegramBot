using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Abstract;
using app.domain.Services;
using app.business.Abstract;
using app.business.Services;

namespace app.web.view.Configure
{
    public class DataLayerConfigure
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IFacultyBusinessService, FacultyBusinessService>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();

            services.AddScoped<ISpecialityBusinessService, SpecialityBusinessService>();
            services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            // services.AddScoped<IWebParseService, WebParseService>();
            services.AddScoped<ITestBusinessService, TestBusinessService>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ITestScoreBusinessService, TestScoreBusinessService>();
            services.AddScoped<ITestScoreRepository, TestScoreRepository>();
            services.AddScoped<IQuestionBusinessService, QuestionBusinessService>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
        }
    }
}