export const load: import('./$types').LayoutLoad = ((event) => {
      return {
            npm_package_name: event.data.npm_package_name,
            npm_package_version: event.data.npm_package_version,
            npm_package_github_url: event.data.npm_package_github_url,
      }
});