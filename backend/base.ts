export abstract class ClientBase {
      protected async transformOptions(options: RequestInit): Promise<RequestInit> {
            // Automatically include credentials (cookies) with all requests
            // This ensures JWT tokens stored in HTTP-only cookies are sent to the backend
            // No need to pass cookie options to frontend - handled automatically
            options.credentials = "include";
            return options;
      }
      protected transformResult(url: string, response: Response, process: (response: Response) => Promise<any>): Promise<any> {
            // Modify response here (e.g., logging)
            return process(response);
      }
}