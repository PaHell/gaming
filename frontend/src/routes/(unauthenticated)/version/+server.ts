import { npm_package_name, npm_package_version } from '$env/static/private';
import { json } from '@sveltejs/kit';

export function GET() {
      return json({
            name: npm_package_name,
            version: npm_package_version
      }, {
            status: 200
      });
}