import { AuthenticationResultErrorModel, CommentVote, PostDetailsModel, VoteType } from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import commentVotesClient from "../../comments/client";

const downvoteComment = async (commentId: string) => {
   await sleep(500);

   const { data, headers, status } = await commentVotesClient.post<CommentVote>(
      `/down/${commentId}`,
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers, status };
};

export const useDownvoteCommentMutation = (postId: string) => {
   const postDetailsKey = ["post", postId];
   const queryClient = useQueryClient();

   return useMutation(downvoteComment, {
      onMutate: async (commentId) => {

         await queryClient.cancelQueries(postDetailsKey);
         const prevPost = queryClient.getQueryData<PostDetailsModel>(postDetailsKey);

         queryClient.setQueryData(postDetailsKey, (prev: PostDetailsModel | undefined) => {
            return {
               ...prev, comments: prev?.comments?.map(c => {
                  return c.id === commentId ?
                     {
                        ...c,
                        voteScore: c.userVote?.type === "Up" ? c.voteScore! - 2 : c.voteScore! - 1,
                        userVote: { ...c.userVote, type: VoteType["1"] },
                     }
                     : c;
               }),
            };
         });

         return { prevPost };
      },
      onError: (_, __, context) => {
         queryClient.setQueryData(postDetailsKey, context!.prevPost!);
      },
      onSuccess: ({ data }) => {
      },
      onSettled: (res) => console.log(res),
   });
};
