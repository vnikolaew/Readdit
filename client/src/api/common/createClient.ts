import axios, { AxiosRequestConfig } from "axios";
import { HttpStatusCode } from "./httpStatusCodes";
import { AuthenticationResultSuccessModel } from "../models";
import { queryClient } from "../../client";

const source = axios.CancelToken.source();

export const createClient = (
   endpoint: string = "",
   config?: AxiosRequestConfig,
   bearerTokenFactory?: () => string | Promise<string>
) => {
   const client = axios.create({
      baseURL: `${process.env.REACT_APP_BASE_API_URL}/${endpoint}`,
      headers: {
         "Content-Type": "application/json; charset=utf-8",
         Accept: "*/*",
      },
      validateStatus: (status) => status < HttpStatusCode.InternalServerError,
      withCredentials: true,
      cancelToken: source.token,
      ...config,
   });

   client.interceptors.request.use(async (config) => {
      let bearerToken;
      if (bearerTokenFactory) {
         let result = bearerTokenFactory();
         bearerToken = result instanceof Promise ? await result : result;
      } else {
         const authToken =
            queryClient.getQueryData<AuthenticationResultSuccessModel>(
               ["user"],
               { exact: true }
            )?.token;

         const expirationTime = authToken?.expiresAt!;
         if (Date.parse(expirationTime) < Date.now()) {
            return config;
         }
         bearerToken = authToken?.value;
      }

      config.headers = {
         ...config.headers,
         Authorization: `Bearer ${bearerToken}`,
      };
      return config;
   });

   return client;
};
