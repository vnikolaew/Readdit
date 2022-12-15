import { AuthenticationResultErrorModel, CountryModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import countryClient from "../client";

const getAllCountries = async () => {
   await sleep(500);

   const { data, status } = await countryClient.get<CountryModel[]>(`/`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetAllCountriesQuery = () => {
   return useQuery(["countries"], getAllCountries, {
      onError: console.error,
      onSuccess: (data) => {},
      onSettled: (res) => console.log(res),
      cacheTime: 10 * 60 * 1000,
      staleTime: 10 * 60 * 1000,
   });
};
