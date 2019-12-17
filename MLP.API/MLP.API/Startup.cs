using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
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
using System.IO;
using Microsoft.AspNetCore.Mvc.Formatters;

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

                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                setupAction.ReturnHttpNotAcceptable = true; //406

                var jsonOutputFormatter = setupAction.OutputFormatters
                    .OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    // remove text/json as it isn't the approved media type
                    // for working with JSON at API level
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
                //setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            var connectionString = Configuration["connectionStrings:DefaultConnection"];
            services.AddDbContext<MLPContext>(o => o.UseSqlServer(connectionString));

            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext = actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            // register the repository
            services.AddScoped<IMLPRepository, MLPRepository>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext =
                implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("MLPOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MLP API",
                    Version = "1.0",
                    Description = "API for creating and handling MLPs",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Joserdz4733@gmail.com",
                        Name = "José Rodríguez Arias"
                    }
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                //var xmlCommentsServicesFile = $"MLP.Services.xml";
                //var xmlCommentsServicesFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsServicesFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
                //setupAction.IncludeXmlComments(xmlCommentsServicesFullPath);
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
                app.UseHsts();
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

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/MLP/swagger/MLPOpenAPISpecification/swagger.json",
                    "MLP API");
                setupAction.RoutePrefix = "";
            });

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
