﻿using Api.MiddleWare;
using Application.Comments.Mappers;
using Application.Comments.Services;
using Application.PostLikes.Mappers;
using Application.PostLikes.Services;
using Application.Posts.Mappers.InputModelMapper;
using Application.Posts.Mappers.ViewModelMapper;
using Application.Posts.Services;
using Application.Tags.Mappers;
using Application.Tags.Services;
using Core.HttpLogic;
using Core.Logs;
using Core.TraceIdLogic.TraceIdAccessors;
using Core.TraceLogic.TraceReaders;
using Domain.Clients.PostUsersInfo;
using Domain.Comments;
using Domain.PostLikes;
using Domain.Posts;
using Domain.Tags;
using IdentityConnectionLib;
using Infrastructure;
using Infrastructure.Comments;
using Infrastructure.PostLikes;
using Infrastructure.Posts;
using Infrastructure.Posts.Connections;
using Infrastructure.Tags;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabase(services);
            ConfigureControllers(services);
            ConfigureSwagger(services);
            AddMappers(services);
            AddHttpRequestServices(services);
            AddLoggerServices(services);
            AddTracingServices(services);
            AddRepositories(services);
            AddConnections(services);
            AddServices(services);
            AddMiddleWares(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PostServiceContext>(options => options.UseNpgsql(connection));
        }

        private static void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;
            }).AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ICommentRepository, CommentsRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPostLikeService, PostLikeService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITagService, TagService>();
        }

        private static void AddConnections(IServiceCollection services)
        {
            services.AddScoped<IPostUserInfoServiceClient, PostUserInfoServiceClient>();
        }

        private static void AddHttpRequestServices(IServiceCollection services)
        {
            services.AddHttpServices();
            services.AddIdentityConnectionLibServices();
        }

        private static void AddTracingServices(IServiceCollection services)
        {
            services.AddTraceId();
        }

        private static void AddLoggerServices(IServiceCollection services)
        {
            services.AddLoggerServices();
        }

        private static void AddMappers(IServiceCollection services)
        {
            services.AddScoped<ICommentViewModelMapper, CommentViewModelMapper>();
            services.AddScoped<IPostLikeViewModelMapper, PostLikeViewModelMapper>();
            services.AddScoped<ITagViewModelMapper, TagViewModelMapper>();
            services.AddScoped<IPostInputModelMapper, PostInputModelMapper>();
            services.AddScoped<IPostViewModelMapper, PostViewModelMapper>();
        }

        private static void AddMiddleWares(IServiceCollection services)
        {
            services.AddTransient<ExceptionHandlingMiddleware>();
        }
    }
}
