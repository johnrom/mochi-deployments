# `nmbl-vercel-dotnet`

|        |         |
| ------ | ------- |
| Status | `alpha` |
| Currently supports | `OrchardCore@1.0.0-rc1-12542` |

Proof of concept: Vercel deployments for DotNet Core. Consists of a base Library and an Orchard Core implementation.

Use to create new `GatsbyJS` or other static site generator builds which query the CMS each time the CMS content is published.

## `Nmbl.Vercel.OcModule`

Orchard Core module implementation of Nmbl.Vercel.

### Getting Started

First, make sure you're using the version of Orchard Core listed above, and add `Nmbl.Vercel.OcModule` to your project.

```
dotnet add nmbl-vercel-dotnet@0.0.1-*
```

Then, add some Vercel configuration to your appSettings.

> Note: doesn't currently support configuration via Orchard Admin.

```json
{
  "VercelOptions": {
    "TeamId": "",
    "ApiToken": "",
    "DeployHook": ""
  },
  "VercelDeploymentOptions": {
    "DeployOnPublish": false,
  }
}
```

### `VercelDeploymentOptions`

`VercelDeploymentOptions` configure some scenarios where publishing the site will automatically run builds in Vercel.

| Setting | Required? | Default Value | Description |
| ------- | --------- | ------------- | ----------- |
| `DeployOnPublish` | | false | Run a deployment when hitting Publish on any ContentItem |

### `VercelOptions`

See [Vercel Options](#NmblVercelVercelOptions).

## `Nmbl.Vercel`

Base library for implementing Vercel Deployments

### `Nmbl.Vercel.VercelOptions`

The default configuration uses some retry and circuit breaker conditions.

| Setting | Required? | Default Value | Description |
| ------- | ----------| ------------- | ----------- |
| `BaseAddress` | x | https://api.vercel.com | The base address of the Vercel endpoint. |
| `DeployHook` | x | | Vercel Deploy Hook Key, required for running deployments. |
| `ApiToken` | x | | Vercel Api Token for reading Deployment Info. |
| `TeamId` | | | ID of your Vercel Team, if applicable.
| `HttpPolicyRetryCount` | | 3 | Number of times to retry requests after transient http failures. |
| `CircuitBreakerPolicyCount` | | 3 | Number of times to fail before breaking, for `CircuitBreakerPolicyIntervalInSeconds`. |
| `CircuitBreakerPolicyIntervalInSeconds` | | 60 | Seconds to break when Circuit Breaker is "open". |
