import React, { FC } from "react";
import { Avatar, Flex, Text } from "@chakra-ui/react";
import { PostCommentDetailsModel } from "../../api/models";
import { Link } from "react-router-dom";
import moment from "moment/moment";
import CommentVotingSection from "./CommentVotingSection";

interface IProps {
   postId: string;
   comment: PostCommentDetailsModel;
}

const PostComment: FC<IProps> = ({ comment, postId }) => {
   return (
      <Flex width={"full"} alignItems={"flex-start"}>
         <Flex justifyContent={"flex-start"} direction={"column"}>
            <Avatar
               boxSize={8}
               src={comment?.author?.profilePictureUrl!}
               borderRadius={"full"}
               objectFit={"cover"}
            />
         </Flex>
         <Flex ml={1} flex={1} px={2} color={"white"} bgColor={"transparent"} direction={"column"}>
            <Flex my={1} display={"flex"} alignItems={"center"} gap={1}>
               <Text color={"gray"} fontSize={12}>
                  <Link to={`/user/${comment.author!.id}`}>
                     <Text pr={2} color={"white"} fontSize={14} display={"inline"}
                           _hover={{ textDecoration: "underline" }}>
                        {comment.author?.userName!}
                     </Text>
                  </Link>
                  <Text display={"inline"} fontSize={14}>
                     ·
                  </Text>
                  <Text px={2} display={"inline"}>
                     {moment(Date.parse(comment!.createdOn!)).fromNow()}
                  </Text>
               </Text>
            </Flex>
            <Text textAlign={"start"} fontSize={14}>{comment.content!}</Text>
            <CommentVotingSection
               postId={postId}
               userVote={comment.userVote!}
               commentId={comment.id!}
               voteScore={comment.voteScore!} orientation={"horizontal"} />
         </Flex>
      </Flex>
   );
};

export default PostComment;