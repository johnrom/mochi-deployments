# `Nmbl.Deployments`

|        |         |
| ------ | ------- |
| Status | `alpha` |
| Currently supports | `OrchardCore@1.0.0-rc1-12542` |

Frontend deployments for DotNet Core. Consists of a base Library, an Orchard Core library, a Vercel library, and an Orchard Core Module to connect them. Use to create new [GatsbyJS](https://www.gatsbyjs.org/) or other static site generator builds which query the CMS each time the CMS content is published.

### Getting Started with OrchardCore and Vercel Front-end Deployments

First, make sure you're using the version of Orchard Core listed above, and add `Nmbl.Deployments.OrchardCore.Vercel` to your project.

```
dotnet add Nmbl.Deployments.OrchardCore.Vercel@0.0.1-*
```

Then, add some Vercel configuration to your appSettings.

> Note: doesn't currently support configuration via Orchard Admin.

```jsonc
{
  "VercelOptions": {
    "TeamId": "",
    "ApiToken": "",
    "DeployHook": ""
  },
  "OrchardDeploymentOptions": {
    "DeployOnPublish": false, // set to true to deploy after Publishing ContentItems.
  }
}
```

Now you can navigate to `Admin -> Content -> Deployments` and list your latest deployments or click "Deploy Now" to run a new deployment.

## `Nmbl.Deployments.Core`

The core deployments library. Has a few options related to [Polly](https://github.com/App-vNext/Polly) policies.

### `DeploymentOptions`

| `HttpPolicyRetryCount` | | 3 | Number of times to retry requests after transient http failures, with exponential back-off. |
| `CircuitBreakerPolicyCount` | | 3 | Number of times to fail before breaking, for `CircuitBreakerPolicyIntervalInSeconds`. |
| `CircuitBreakerPolicyIntervalInSeconds` | | 60 | Seconds to break when Circuit Breaker is "open". |

## `Nmbl.Deployments.OrchardCore`

Base OrchardCore library connecting the Core library to Orchard Cache and Publish process.

### `OrchardDeploymentOptions`

`OrchardDeploymentOptions` configures whether publishing the site will automatically run deployments.

| Setting | Required? | Default Value | Description |
| ------- | --------- | ------------- | ----------- |
| `DeployOnPublish` | | false | Run a deployment when hitting Publish on any ContentItem |

## `Nmbl.Deployments.Vercel`

Base Vercel library connecting the Core library to Vercel API endpoints.

### `VercelOptions`

| Setting | Required? | Default Value | Description |
| ------- | ----------| ------------- | ----------- |
| `BaseAddress` | x | https://api.vercel.com | The base address of the Vercel endpoint. |
| `DeployHook` | x | | Vercel Deploy Hook Key, required for running deployments. |
| `ApiToken` | x | | Vercel Api Token for reading Deployment Info. |
| `TeamId` | | | ID of your Vercel Team, if applicable.

## `Nmbl.Deployments.OrchardCore.Vercel`

Orchard Core Module connecting `Nmbl.Deployments.Vercel` and `Nmbl.Deployments.OrchardCore`. No configuration of its own.
