import {
   AuthenticationResultErrorModel,
   PostApiCommentsBody,
   PostComment,
   PostDetailsModel,
   UserDetailsModel,
} from "../../models";
import { sleep } from "../../../utils/sleep";
import { HttpStatusCode } from "../../common/httpStatusCodes";
import { ApiError } from "../../common/ApiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import commentsClient from "../client";
import { useCurrentUser } from "../../common/hooks/useCurrentUser";

const createComment = async (payload: PostApiCommentsBody) => {
   await sleep(500);

   const { data, headers, status } = await commentsClient.post<PostComment>(
      `/`, payload,
   );

   if (status !== HttpStatusCode.OK) {
      throw new ApiError((data as AuthenticationResultErrorModel).errors!);
   }

   return { data, headers };
};

export const useCreateCommentMutation = () => {
   const queryClient = useQueryClient();
   const user = useCurrentUser();

   return useMutation(createComment, {
      onMutate: async ({ Content, PostId }) => {
         const postDetailsKey = ["post", PostId];

         await queryClient.cancelQueries(postDetailsKey);

         const prevPost = queryClient.getQueryData<PostDetailsModel>(postDetailsKey);
         const currentUserDetails = queryClient.getQueryData<UserDetailsModel>(["user", "details", user?.userId!]);

         queryClient.setQueryData(postDetailsKey, (prev: PostDetailsModel | undefined) => {
            return ({
               ...prev, comments: [{
                  id: "",
                  createdOn: new Date().toLocaleString(),
                  voteScore: 0,
                  userVote: undefined,
                  author: {
                     id: user?.userId,
                     userName: currentUserDetails!.userName!,
                     profilePicturePublicId: currentUserDetails!.profile?.pictureUrl!,
                     profilePictureUrl: currentUserDetails!.profile?.picturePublicId,
                  },
                  content: Content,
                  modifiedOn: undefined,
               }, ...(prev?.comments || [])],
            });
         });
         return { prevPost };
      },
      onError: (error, variables, context) => {
         queryClient.setQueryData<PostDetailsModel>(["post", context!.prevPost!.id!], context!.prevPost);
      },
      onSuccess: ({ data }, payload, context) => {
      },
      onSettled: (res) => console.log(res),
   });
};
