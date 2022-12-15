import { AuthenticationResultErrorModel, PostVote } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation } from "@tanstack/react-query";
import commentVotesClient from "../../comments/client";

const removeCommentUpvote = async (commentId: string) => {
   await sleep(500);

   const { data, headers, status } = await commentVotesClient.delete<PostVote>(
      `/up/${commentId}`
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useRemoveUpvoteCommentMutation = () => {
   return useMutation(removeCommentUpvote, {
      onError: console.error,
      onSuccess: ({ data }) => {},
      onSettled: (res) => console.log(res),
   });
};
