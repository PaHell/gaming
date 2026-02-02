// src/routes/auth/register/+page.server.ts
import { AuthenticationClient, type RegisterForm } from '@/clients';
import type { Actions } from './$types';
import { fail, redirect } from '@sveltejs/kit';
import { env } from '$env/dynamic/public';

export const actions: Actions = {
      default: async ({ request, fetch }) => {
            const formData = await request.formData();
            const values: RegisterForm = {
                  firstName: formData.get('firstName')?.toString(),
                  lastName: formData.get('lastName')?.toString(),
                  email: formData.get('email')?.toString(),
                  password: formData.get('password')?.toString()
            };
            try {
                  const session = await new AuthenticationClient(env.PUBLIC_BACKEND_URL_SERVER, { fetch }).register(values);
                  console.log({ session });
                  throw redirect(303, '/app');
            } catch (error) {
                  console.error(error);
                  if (error instanceof Error) {
                        return fail(400, {
                              error: `${error.name}: ${error.message}`,
                              values,
                        });
                  }
                  return fail(400, { values, error });
            }

      }
};
