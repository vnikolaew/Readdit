import React, { FC } from "react";
import { PostCommentDetailsModel } from "../../api/models";
import Link from "../../components/Link";
import moment from "moment/moment";
import CommentVotingSection from "./CommentVotingSection";
import { Avatar, Flex, Text, useMantineTheme } from "@mantine/core";

interface IProps {
   postId: string;
   comment: PostCommentDetailsModel;
}

const PostComment: FC<IProps> = ({ comment, postId }) => {
   const theme = useMantineTheme();

   return (
      <Flex w={"100%"} align={"flex-start"}>
         <Flex justify={"flex-start"} direction={"column"}>
            <Avatar size={24} src={comment?.author?.profilePictureUrl!} radius={"lg"} />
         </Flex>
         <Flex
            style={{ flexGrow: 1 }}
            ml={1}
            px={8}
            color={theme.colors.gray[0]}
            gap={6}
            bg={"transparent"}
            direction={"column"}
         >
            <Flex my={1} align={"center"} gap={1}>
               <Text color={theme.colors.gray[6]} fz={12}>
                  <Link to={`/user/${comment.author!.id}`}>
                     <Text span pr={8} color={theme.colors.dark[0]} fz={14}>
                        {comment.author?.userName!}
                     </Text>
                  </Link>
                  <Text span fz={14}>
                     ·
                  </Text>
                  <Text span px={8}>
                     {moment(Date.parse(comment!.createdOn!)).fromNow()}
                  </Text>
               </Text>
            </Flex>
            <Text color={theme.colors.gray[0]} ta={"start"} fz={14}>
               {comment.content!}
            </Text>
            <CommentVotingSection
               postId={postId}
               userVote={comment.userVote!}
               commentId={comment.id!}
               voteScore={comment.voteScore!}
               orientation={"horizontal"}
            />
         </Flex>
      </Flex>
   );
};

export default PostComment;