// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Model.AppveyorDeployment;
using System;

#nullable disable
public class DeplymentRoot
{
    public Project project { get; set; }
    public Deployment[] deployments { get; set; }
}

public class Project
{
    public int projectId { get; set; }
    public int accountId { get; set; }
    public string accountName { get; set; }
    public object[] builds { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public string repositoryType { get; set; }
    public string repositoryScm { get; set; }
    public string repositoryName { get; set; }
    public bool isPrivate { get; set; }
    public bool skipBranchesWithoutAppveyorYml { get; set; }
    public Securitydescriptor securityDescriptor { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Securitydescriptor
{
}

public class Deployment
{
    public Environment environment { get; set; }
    public Deployment1 deployment { get; set; }
}

public class Environment
{
    public int deploymentEnvironmentId { get; set; }
    public string name { get; set; }
    public string provider { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Deployment1
{
    public int deploymentId { get; set; }
    public Build build { get; set; }
    public Environment1 environment { get; set; }
    public Job[] jobs { get; set; }
    public string status { get; set; }
    public DateTime started { get; set; }
    public DateTime finished { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Build
{
    public int buildId { get; set; }
    public object[] jobs { get; set; }
    public int buildNumber { get; set; }
    public string version { get; set; }
    public string message { get; set; }
    public string branch { get; set; }
    public string commitId { get; set; }
    public string authorName { get; set; }
    public string authorUsername { get; set; }
    public string committerName { get; set; }
    public string committerUsername { get; set; }
    public DateTime committed { get; set; }
    public object[] messages { get; set; }
    public string status { get; set; }
    public DateTime started { get; set; }
    public DateTime finished { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Environment1
{
    public int deploymentEnvironmentId { get; set; }
    public string name { get; set; }
    public string provider { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Job
{
    public string jobId { get; set; }
    public string name { get; set; }
    public int messagesCount { get; set; }
    public string status { get; set; }
    public DateTime started { get; set; }
    public DateTime finished { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

#nullable restore
