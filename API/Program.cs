using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Application.Interfaces;
using Application.Services;
using API.Extensions;
using Application.Authorization;
using Application.Utils;
using DataAccess.Repositories.PageRepositories;
using DataAccess.Interfaces.PageRepositories;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards;
using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using DataAccess.Repositories.PageRepositories.FinalPerformanceAccounting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Old
//builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerAuth();

//New
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"])),
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

//Old
//builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CuratorsJournalDBContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    });

builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IJournalsRepository, JournalsRepository>();
builder.Services.AddScoped<ICuratorsRepository, CuratorsRepository>();
builder.Services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();
builder.Services.AddScoped<IContactPhonesRepository, ContactPhonesRepository>();
builder.Services.AddScoped<IHolidaysRepository, HolidaysRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICuratorsAppointmentHistoryRepository, CuratorsAppointmentHistoryRepository>();
builder.Services.AddScoped<IPagesRepository, PagesRepository>();
builder.Services.AddScoped<IEducationalProcessScheduleRepository, EducationalProcessScheduleRepository>();
builder.Services.AddScoped<IGroupActivesRepository, GroupActivesRepository>();
builder.Services.AddScoped<IStudentListRepository, StudentListRepository>();
builder.Services.AddScoped<IStudentHealthCardRepository, StudentHealthCardRepository>();
builder.Services.AddScoped<IIdeologicalEducationalWorkRepository, IdeologicalEducationalWorkRepository>();
builder.Services.AddScoped<IParticipationInPedagogicalSeminarsRepository, ParticipationInPedagogicalSeminarsRepository>();
builder.Services.AddScoped<ILiteratureRepository, LiteratureRepository>();
builder.Services.AddScoped<ILiteratureWorkRepository, LiteratureWorkRepository>();
builder.Services.AddScoped<IPsychologicalAndPedagogicalCharacteristicsRepository, PsychologicalAndPedagogicalCharacteristicsRepository>();
builder.Services.AddScoped<IRecommendationsAndRemarksRepository, RecommendationsAndRemarksRepository>();
builder.Services.AddScoped<ITraditionsRepository, TraditionsRepository>();
builder.Services.AddScoped<IDynamicsOfKeyIndicatorsRepository, DynamicsOfKeyIndicatorsRepository>();
builder.Services.AddScoped<ISocioPedagogicalCharacteristicsRepository, SocioPedagogicalCharacteristicsRepository>();
builder.Services.AddScoped<IPersonalizedAccountingCardsRepository, PersonalizedAccountingCardsRepository>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddScoped<IParentalInformationRepository, ParentalInformationRepository>();
builder.Services.AddScoped<IIndividualInformationRepository, IndividualInformationRepository>();
builder.Services.AddScoped<IStudentEncouragementsRepository, StudentEncouragementsRepository>();
builder.Services.AddScoped<IStudentDisciplinaryResponsibilitiesRepository, StudentDisciplinaryResponsibilitiesRepository>();
builder.Services.AddScoped<IWorkWithParentsRepository, WorkWithParentsRepository>();
builder.Services.AddScoped<IIndividualWorkWithStudentRepository, IndividualWorkWithStudentRepository>();
builder.Services.AddScoped<ISocioPedagogicalCharacteristicsAttributesRepository, SocioPedagogicalCharacteristicsAttributesRepository>();
builder.Services.AddScoped<IHealthCardAttributesRepository, HealthCardAttributesRepository>();
builder.Services.AddScoped<IInformationHoursAccountingRepository, InformationHoursAccountingRepository>();
builder.Services.AddScoped<IWorkersRepository, WorkersRepository>();
builder.Services.AddScoped<IAcademicYearsRepository, AcademicYearsRepository>();
builder.Services.AddScoped<IFacultiesRepository, FacultiesRepository>();
builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();
builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
builder.Services.AddScoped<IChronicDiseasesRepository, ChronicDiseasesRepository>();
builder.Services.AddScoped<IPEGroupsRepository, PEGroupsRepository>();
builder.Services.AddScoped<IDeaneriesRepository, DeaneriesRepository>();
builder.Services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
builder.Services.AddScoped<ICertificationTypesRepository, CertificationTypesRepository>();
builder.Services.AddScoped<IPerformanceAccountingRecordsRepository, PerformanceAccountingRecordsRepository>();
builder.Services.AddScoped<IIdeologicalAndEducationalWorkPageAttributesRepository, IdeologicalAndEducationalWorkPageAttributesRepository>();
builder.Services.AddScoped<IPerformanceAccountingGradesRepository, PerformanceAccountingGradesRepository>();
builder.Services.AddScoped<IPerformanceAccountingColumnsRepository, PerformanceAccountingColumnsRepository>();
builder.Services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IJournalsService, JournalsService>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7246")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

//Old
//app.UseCookiePolicy(new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.Strict,
//    HttpOnly = HttpOnlyPolicy.Always,
//    Secure = CookieSecurePolicy.Always
//});

app.UseCors();

//Old
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
