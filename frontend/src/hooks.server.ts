import { env } from '$env/dynamic/public';
import { AuthenticationClient } from '@/clients';
import { json, redirect, type Handle, type HandleFetch, type HandleServerError } from '@sveltejs/kit';
import { jwtDecode } from "jwt-decode";
import { unknownErrorToString } from './lib';

const ACCESS_TOKEN = "access_token";
const REFRESH_TOKEN = "refresh_token";

export const handle: Handle = async ({ event, resolve }) => {
      console.log(`[handle] (${event.url.pathname})`);
      if (!event.route.id?.startsWith('/(unauthenticated)')) {
            const accessToken = event.cookies.get(ACCESS_TOKEN);
            const refreshToken = event.cookies.get(REFRESH_TOKEN);

            if (!accessToken || !refreshToken) throw redirectToAuthentication(event.url.pathname);

            if (isTokenExpired(accessToken)) {
                  const client = new AuthenticationClient(env.PUBLIC_BACKEND_URL_SERVER, { fetch: event.fetch });
                  await client.refreshToken();
            }
      }

      const response = await resolve(event);
      return response;
};

export const handleFetch: HandleFetch = async ({ event, request, fetch }) => {
      try {
            let response = await fetch(request);
            console.log(`[handleFetch] (${event.url.pathname}): ${response.status}`);

            if (response.status === 401 && !event.url.pathname.startsWith("/auth")) {
                  const accessToken = event.cookies.get(ACCESS_TOKEN);
                  if (accessToken && isTokenExpired(accessToken)) {
                        const client = new AuthenticationClient(env.PUBLIC_BACKEND_URL_SERVER, { fetch });
                        await client.refreshToken();
                        response = await fetch(request);
                  }
            }

            return response;
      } catch (error) {
            console.error(`[handleFetch] (${event.url.pathname}): ERROR: ${error}`);
            return json({ error: unknownErrorToString(error) });
      }
};

export const handleError: HandleServerError = ({ error, event, status, message }) => {
      console.error(`[handleError] (${event.url.pathname}): ${status} ${message} ${error}`);
      if (status == 401 && !event.url.pathname.startsWith("/auth")) {
            return redirectToAuthentication(event.url.pathname);
      }
      return new Error(unknownErrorToString(error));
};

function isTokenExpired(accessToken: string) {
      console.log(`  -> isTokenExpired`);
      const claims = jwtDecode<{
            exp: number
      }>(accessToken);

      return claims.exp < Math.floor(Date.now() / 1000);
}

function redirectToAuthentication(returnUrl: string) {
      console.log(`  -> redirectToAuthentication`);
      if (!returnUrl || returnUrl == "/") {
            throw redirect(307, "/auth/login");
      }
      throw redirect(307, "/auth/login?returnUrl=" + encodeURIComponent(returnUrl));
}