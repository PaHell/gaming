// place files you want to import through the `$lib` alias in this folder.
export function unknownErrorToString(error: unknown): string {
      if (error instanceof Error) {
            return `${error.name}: ${error.message}`;
      }
      switch (typeof (error)) {
            case "object": {
                  if (!error) return "Error!";
                  const byTypicalProp = error?.message ?? error?.error;
                  if (byTypicalProp) return byTypicalProp;
                  const keys = Object.keys(error);
                  if (keys.length) return error[keys[0]];
                  return "Error!";
            }
            case "string":
                  return error;
            case "number":
            case "bigint":
                  return `Error No.${error}`;
            case "symbol":
                  return `Error No.${error.toString()}`;
            case "function":
                  return error?.()?.toString() ?? "Error!";
            case "boolean":
            case "undefined":
                  return `Error!`;
      }
}

type CookieOptions = import('cookie').CookieSerializeOptions & { path: string };

export function getCookieOptions(): CookieOptions {
      return {
            httpOnly: true,
            sameSite: "strict",
            path: "/",
            secure: true,
            domain: "localhost"
      };
}