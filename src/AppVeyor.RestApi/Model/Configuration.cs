// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

#nullable disable
namespace AppVeyor.RestApi.Model;

using Newtonsoft.Json;

public class Configuration
{
    [JsonProperty("isBaseConfig")]
    public bool IsBaseConfig { get; set; }

    [JsonProperty("doNotIncrementBuildNumberOnPullRequests")]
    public bool DoNotIncrementBuildNumberOnPullRequests { get; set; }

    [JsonProperty("hotFixScripts")]
    public List<object> HotFixScripts { get; set; }

    [JsonProperty("initScripts")]
    public List<object> InitScripts { get; set; }

    [JsonProperty("branchesMode")]
    public string BranchesMode { get; set; }

    [JsonProperty("includeBranches")]
    public List<IncludeBranch> IncludeBranches { get; set; }

    [JsonProperty("excludeBranches")]
    public List<object> ExcludeBranches { get; set; }

    [JsonProperty("skipTags")]
    public bool SkipTags { get; set; }

    [JsonProperty("skipNonTags")]
    public bool SkipNonTags { get; set; }

    [JsonProperty("skipBranchWithPullRequests")]
    public bool SkipBranchWithPullRequests { get; set; }

    [JsonProperty("skipCommitsFiles")]
    public List<object> SkipCommitsFiles { get; set; }

    [JsonProperty("onlyCommitsFiles")]
    public List<object> OnlyCommitsFiles { get; set; }

    [JsonProperty("cloneScripts")]
    public List<object> CloneScripts { get; set; }

    [JsonProperty("onBuildSuccessScripts")]
    public List<object> OnBuildSuccessScripts { get; set; }

    [JsonProperty("onBuildErrorScripts")]
    public List<object> OnBuildErrorScripts { get; set; }

    [JsonProperty("onBuildFinishScripts")]
    public List<object> OnBuildFinishScripts { get; set; }

    [JsonProperty("patchAssemblyInfo")]
    public bool PatchAssemblyInfo { get; set; }

    [JsonProperty("assemblyInfoFile")]
    public string AssemblyInfoFile { get; set; }

    [JsonProperty("assemblyVersionFormat")]
    public string AssemblyVersionFormat { get; set; }

    [JsonProperty("assemblyFileVersionFormat")]
    public string AssemblyFileVersionFormat { get; set; }

    [JsonProperty("assemblyInformationalVersionFormat")]
    public string AssemblyInformationalVersionFormat { get; set; }

    [JsonProperty("patchDotnetCsproj")]
    public bool PatchDotnetCsproj { get; set; }

    [JsonProperty("dotnetCsprojFile")]
    public string DotnetCsprojFile { get; set; }

    [JsonProperty("dotnetCsprojVersionFormat")]
    public string DotnetCsprojVersionFormat { get; set; }

    [JsonProperty("dotnetCsprojVersionPrefixFormat")]
    public string DotnetCsprojVersionPrefixFormat { get; set; }

    [JsonProperty("dotnetCsprojPackageVersionFormat")]
    public string DotnetCsprojPackageVersionFormat { get; set; }

    [JsonProperty("dotnetCsprojAssemblyVersionFormat")]
    public string DotnetCsprojAssemblyVersionFormat { get; set; }

    [JsonProperty("dotnetCsprojFileVersionFormat")]
    public string DotnetCsprojFileVersionFormat { get; set; }

    [JsonProperty("dotnetCsprojInformationalVersionFormat")]
    public string DotnetCsprojInformationalVersionFormat { get; set; }

    [JsonProperty("buildCloud")]
    public List<object> BuildCloud { get; set; }

    [JsonProperty("operatingSystem")]
    public List<object> OperatingSystem { get; set; }

    [JsonProperty("services")]
    public List<object> Services { get; set; }

    [JsonProperty("stacks")]
    public List<object> Stacks { get; set; }

    [JsonProperty("shallowClone")]
    public bool ShallowClone { get; set; }

    [JsonProperty("forceHttpsClone")]
    public bool ForceHttpsClone { get; set; }

    [JsonProperty("environmentVariables")]
    public List<EnvironmentVariable> EnvironmentVariables { get; set; }

    [JsonProperty("environmentVariablesMatrix")]
    public List<object> EnvironmentVariablesMatrix { get; set; }

    [JsonProperty("installScripts")]
    public List<object> InstallScripts { get; set; }

    [JsonProperty("hostsEntries")]
    public List<object> HostsEntries { get; set; }

    [JsonProperty("cacheEntries")]
    public List<object> CacheEntries { get; set; }

    [JsonProperty("configureNuGetProjectSource")]
    public bool ConfigureNuGetProjectSource { get; set; }

    [JsonProperty("configureNuGetAccountSource")]
    public bool ConfigureNuGetAccountSource { get; set; }

    [JsonProperty("disableNuGetPublishOnPullRequests")]
    public bool DisableNuGetPublishOnPullRequests { get; set; }

    [JsonProperty("disableNuGetPublishForOctopusPackages")]
    public bool DisableNuGetPublishForOctopusPackages { get; set; }

    [JsonProperty("buildMode")]
    public string BuildMode { get; set; }

    [JsonProperty("platform")]
    public List<object> Platform { get; set; }

    [JsonProperty("configuration")]
    public List<object> Configuration1 { get; set; }

    [JsonProperty("packageWebApplicationProjects")]
    public bool PackageWebApplicationProjects { get; set; }

    [JsonProperty("packageWebApplicationProjectsXCopy")]
    public bool PackageWebApplicationProjectsXCopy { get; set; }

    [JsonProperty("packageWebApplicationProjectsBeanstalk")]
    public bool PackageWebApplicationProjectsBeanstalk { get; set; }

    [JsonProperty("packageWebApplicationProjectsOctopus")]
    public bool PackageWebApplicationProjectsOctopus { get; set; }

    [JsonProperty("packageAzureWebJobProjects")]
    public bool PackageAzureWebJobProjects { get; set; }

    [JsonProperty("packageAzureCloudServiceProjects")]
    public bool PackageAzureCloudServiceProjects { get; set; }

    [JsonProperty("packageNuGetProjects")]
    public bool PackageNuGetProjects { get; set; }

    [JsonProperty("packageNuGetSymbols")]
    public bool PackageNuGetSymbols { get; set; }

    [JsonProperty("useSnupkgFormat")]
    public bool UseSnupkgFormat { get; set; }

    [JsonProperty("packageAspNetCoreProjects")]
    public bool PackageAspNetCoreProjects { get; set; }

    [JsonProperty("packageDotnetConsoleProjects")]
    public bool PackageDotnetConsoleProjects { get; set; }

    [JsonProperty("includeNuGetReferences")]
    public bool IncludeNuGetReferences { get; set; }

    [JsonProperty("msBuildInParallel")]
    public bool MsBuildInParallel { get; set; }

    [JsonProperty("msBuildVerbosity")]
    public string MsBuildVerbosity { get; set; }

    [JsonProperty("buildScripts")]
    public List<object> BuildScripts { get; set; }

    [JsonProperty("beforeBuildScripts")]
    public List<object> BeforeBuildScripts { get; set; }

    [JsonProperty("beforePackageScripts")]
    public List<object> BeforePackageScripts { get; set; }

    [JsonProperty("afterBuildScripts")]
    public List<object> AfterBuildScripts { get; set; }

    [JsonProperty("testMode")]
    public string TestMode { get; set; }

    [JsonProperty("testAssemblies")]
    public List<object> TestAssemblies { get; set; }

    [JsonProperty("testCategories")]
    public List<object> TestCategories { get; set; }

    [JsonProperty("testCategoriesMatrix")]
    public List<object> TestCategoriesMatrix { get; set; }

    [JsonProperty("testScripts")]
    public List<object> TestScripts { get; set; }

    [JsonProperty("beforeTestScripts")]
    public List<object> BeforeTestScripts { get; set; }

    [JsonProperty("afterTestScripts")]
    public List<object> AfterTestScripts { get; set; }

    [JsonProperty("deployMode")]
    public string DeployMode { get; set; }

    [JsonProperty("deployments")]
    public List<object> Deployments { get; set; }

    [JsonProperty("deployScripts")]
    public List<object> DeployScripts { get; set; }

    [JsonProperty("beforeDeployScripts")]
    public List<object> BeforeDeployScripts { get; set; }

    [JsonProperty("afterDeployScripts")]
    public List<object> AfterDeployScripts { get; set; }

    [JsonProperty("onImageBakeScripts")]
    public List<object> OnImageBakeScripts { get; set; }

    [JsonProperty("xamarinRegisterAndroidProduct")]
    public bool XamarinRegisterAndroidProduct { get; set; }

    [JsonProperty("xamarinRegisterIosProduct")]
    public bool XamarinRegisterIosProduct { get; set; }

    [JsonProperty("matrixFastFinish")]
    public bool MatrixFastFinish { get; set; }

    [JsonProperty("matrixAllowFailures")]
    public List<object> MatrixAllowFailures { get; set; }

    [JsonProperty("matrixExclude")]
    public List<object> MatrixExclude { get; set; }

    [JsonProperty("matrixOnly")]
    public List<object> MatrixOnly { get; set; }

    [JsonProperty("matrixExcept")]
    public List<object> MatrixExcept { get; set; }

    [JsonProperty("artifacts")]
    public List<object> Artifacts { get; set; }

    [JsonProperty("notifications")]
    public List<object> Notifications { get; set; }
}
