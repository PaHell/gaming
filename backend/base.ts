export abstract class ClientBase {
      protected async transformOptions(options: RequestInit): Promise<RequestInit> {
            // Modify request options here (e.g., add headers)
            options.credentials = "include";
            return options;
      }
      protected transformResult(url: string, response: Response, process: (response: Response) => Promise<any>): Promise<any> {
            // Modify response here (e.g., logging)
            return process(response);
      }
}