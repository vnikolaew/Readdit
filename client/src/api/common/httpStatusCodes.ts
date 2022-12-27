export enum HttpStatusCode {
   OK = 200,
   Created = 201,
   Accepted = 202,
   NoContent = 204,
   Found = 302,
   NotModified = 304,
   UseProxy = 305,
   TemporaryRedirect = 307,
   PermanentRedirect = 307,
   BadRequest = 400,
   Unauthorized = 401,
   Forbidden = 403,
   NotFound = 404,
   MethodNotAllowed = 405,
   NotAcceptable = 406,
   RequestTimeout = 408,
   Conflict = 409,
   InternalServerError = 500,
   BadGateway = 502,
   ServiceUnavailable = 503,
}