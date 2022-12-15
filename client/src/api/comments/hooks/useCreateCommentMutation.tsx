import {
   AuthenticationResultErrorModel,
   PostApiCommentsBody,
   PostComment,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentsClient from "../client";

const createComment = async (payload: PostApiCommentsBody) => {
   await sleep(500);

   const { data, headers, status } = await commentsClient.post<PostComment>(
      `/`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useCreateCommentMutation = () => {
   return useMutation(createComment, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
