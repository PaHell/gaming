import { npm_package_name, npm_package_version } from '$env/static/private';
import pkg from '../../package.json';

export const load: import('./$types').LayoutServerLoad = (() => {
      return {
            npm_package_name,
            npm_package_version,
            npm_package_github_url: pkg.repository.url,
      }
});