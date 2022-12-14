import { AuthenticationResultErrorModel, CommunityPostModel } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useQuery } from "@tanstack/react-query";
import postsClient from "../client";

const getAllByCommunity = async (communityId: string) => {
   await sleep(500);

   const { data, status } = await postsClient.get<
      CommunityPostModel[]
   >(`/community/${communityId}`);

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return data;
};

export const useGetAllPostsByCommunityQuery = (communityId: string) => {
   return useQuery(
      ["community", communityId, "posts"],
      () => getAllByCommunity(communityId),
      {
         onError: console.error,
         onSuccess: (data) => {
         },
         onSettled: (res) => console.log(res),
      },
   );
};
