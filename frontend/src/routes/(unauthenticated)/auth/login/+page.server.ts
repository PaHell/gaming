import type { Actions } from './$types';
import { fail, redirect } from '@sveltejs/kit';
import { AuthenticationClient } from '@/clients';
import { env } from '$env/dynamic/public';
import { getCookieOptions, unknownErrorToString } from '@/index';

export const actions: Actions = {
      default: async ({ request, fetch, cookies }) => {
            const formData = await request.formData();

            const values = {
                  email: formData.get('email')?.toString() ?? '',
                  password: formData.get('password')?.toString() ?? ''
            };

            try {
                  const session = await new AuthenticationClient(
                        env.PUBLIC_BACKEND_URL_SERVER,
                        { fetch }
                  ).login(values);

                  console.log({ session });

                  cookies.set("access_token", session.accessToken, getCookieOptions());

                  cookies.set("refresh_token", session.refreshToken, getCookieOptions());

                  throw redirect(303, '/app');
            } catch (error) {
                  console.error(error);
                  return fail(400, { values, error: unknownErrorToString(error) });
            }
      }
};
