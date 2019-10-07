using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MLP.Entities;
using MLP.Services;
using Newtonsoft.Json.Serialization;

namespace MLP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true; //406
                //setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            var connectionString = Configuration["connectionStrings:DefaultConnection"];
            services.AddDbContext<MLPContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<IMLPRepository, MLPRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContect =
                implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContect);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, MLPContext mlpcontext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => {
                    appBuilder.Run(async context => {
                        var exceptionHadlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHadlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHadlerFeature.Error, exceptionHadlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<Entities.NeuralNetwork, Models.OutputModels.NeuralNetworkDto>()
                    .ForMember(dest => dest.HiddenNeurons, opt => opt.MapFrom(src =>
                    src.TrainingConfig.HiddenNeuronElements))
                    .ForMember(dest => dest.OutputNeurons, opt => opt.MapFrom(src =>
                    src.TrainingConfig.OutputNeuronElements));

                cfg.CreateMap<Models.InputModels.NeuralNetworkForCreationDto, Entities.NeuralNetwork>();
                cfg.CreateMap<Models.InputModels.NeuralNetworkTrainingConfigForCreationDto, Entities.NeuralNetworkTrainingConfig>();
                cfg.CreateMap<Models.InputModels.NeuronForCreationDto, Entities.Neuron>();
                cfg.CreateMap<Models.InputModels.NeuronWeightForCreationDto, Entities.NeuronWeight>();
                cfg.CreateMap<Models.Domain.NeuronWeightForManipulation, Entities.NeuronWeight>();
                cfg.CreateMap<Models.InputModels.PredictedObjectForCreationDto, Entities.PredictedObject>();

                cfg.CreateMap<Entities.Neuron, Models.Domain.NeuronForManipulation>();

                cfg.CreateMap<Entities.NeuralNetworkTrainingConfig, Models.OutputModels.NeuralNetworkTrainingConfigDto>();

                cfg.CreateMap<Entities.ImageProcessingConfig, Models.OutputModels.ImageProcessingConfigDto>();
                cfg.CreateMap<Models.InputModels.ImageProcessingConfigForCreationDto, Entities.ImageProcessingConfig>();
                cfg.CreateMap<Models.InputModels.ImageProcessingConfigWithImageForCreationDto, Models.InputModels.ImageProcessingConfigForCreationDto>();
                cfg.CreateMap<Models.OutputModels.ImageProcessingConfigValuesDto, Models.InputModels.ImageProcessingConfigForCreationDto>();

            });
            //mlpcontext.EnsureSeedDataForContext();
            app.UseMvc();
        }
    }
}
