import React, { FC } from "react";
import { Box, Button, Divider, Flex, Spinner, Text } from "@chakra-ui/react";
import { useNavigate, useParams } from "react-router-dom";
import { useGetPostDetailsQuery } from "../../api/posts/hooks/useGetPostDetailsQuery";
import { log } from "../../utils/logger";
import VotingSection from "../../components/VotingSection";
import { ImImage } from "react-icons/im";
import { shorten } from "../../utils/string";
import FeedPost from "../home/feed-post/FeedPost";
import { CloseIcon } from "@chakra-ui/icons";
import CommentsSection from "./CommentsSection";

const PostDetails: FC = () => {
   const { postId } = useParams();
   const navigate = useNavigate();
   const { data: post, isLoading, isError, error } = useGetPostDetailsQuery(postId!);

   if (isLoading || !post) {
      return <Spinner size={"md"} colorScheme={"twitter"} />;
   }

   log(post);
   return (
      <Box width={"70%"} mx={"auto"}>
         <Flex mx={"auto"} alignItems={"center"} direction={"column"} gap={6}>
            <Flex
               bgColor={"blackAlpha.900"}
               position={"sticky"}
               top={-1}
               justifyContent={"center"}
               borderBottomRadius={6}
               boxShadow={"md"}
               p={3}
               px={10}
               width={"full"}
               gap={1}
               alignItems={"center"}>
               <Divider height={4} color={"black"} orientation={"vertical"} />
               <VotingSection
                  userVote={post.userVote!}
                  postId={post.id!}
                  bgColor={"transparent"}
                  color={"white"}
                  voteScore={post.voteScore!}
                  orientation={"horizontal"} />
               <Divider height={4} color={"blackAlpha.900"} orientation={"vertical"} />
               {post.mediaUrl && (
                  <Box mx={3}>
                     <ImImage size={16} color={"white"} />
                  </Box>
               )}
               <Text color={"white"}>{shorten(post.title!, 50)}</Text>
               <Button
                  ml={"auto"}
                  _hover={{}}
                  onClick={() => navigate(-1)}
                  fontSize={12} bgColor={"transparent"} color={"white"}
                  leftIcon={<CloseIcon boxSize={3} color={"white"} />}>
                  Close
               </Button>
            </Flex>
            <FeedPost _hover={{}} width={"600px"} post={{ ...post, commentsCount: post.comments!.length }} />
            <CommentsSection postId={post.id!} comments={post.comments!} />
         </Flex>
      </Box>
   );
};

export default PostDetails;