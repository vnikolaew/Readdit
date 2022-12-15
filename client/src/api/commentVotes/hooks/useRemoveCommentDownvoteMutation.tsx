import { AuthenticationResultErrorModel, PostVote } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentVotesClient from "../../comments/client";

const removeCommentDownvote = async (commentId: string) => {
   await sleep(500);

   const { data, headers, status } = await commentVotesClient.delete<PostVote>(
      `/down/${commentId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useRemoveDownvoteCommentMutation = () => {
   return useMutation(removeCommentDownvote, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
