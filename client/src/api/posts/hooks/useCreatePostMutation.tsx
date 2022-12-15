import {
   AuthenticationResultErrorModel,
   CommunityPost,
   PostApiPostsBody,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import postsClient from "../client";

const createPost = async (model: PostApiPostsBody) => {
   await sleep(500);

   const { data, headers, status } = await postsClient.postForm<CommunityPost>(
      "/",
      model
   );

   if (status !== HttpStatusCode.Created) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useCreatePostMutation = () => {
   return useMutation(createPost, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
