import {
   AuthenticationResultErrorModel,
   CommunityPost,
   PutApiPostsBody,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import postsClient from "../client";

const updatePost = async (postId: string, payload: PutApiPostsBody) => {
   await sleep(500);

   const { data, headers, status } = await postsClient.put<CommunityPost>(
      `/${postId}`,
      payload
   );

   if (status === HttpStatusCode.Accepted) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useUpdatePostMutation = (postId: string) => {
   return useMutation((payload) => updatePost(postId, payload), {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
