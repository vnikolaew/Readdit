import { Avatar, Flex, FlexProps, Image, Text } from "@chakra-ui/react";
import React, { FC } from "react";
import { FeedCommunityPostModel } from "../../../api/models";
import { Link } from "react-router-dom";
import moment from "moment";
import FeedPostButton from "./FeedPostButton";
import { FaRegComment } from "react-icons/fa";
import VotingSection from "../../../components/VotingSection";

interface IProps extends FlexProps {
   post: FeedCommunityPostModel;
}

const FeedPost: FC<IProps> = ({ post, ...rest }) => {
   return (
      <Flex borderWidth={2}
            borderColor={"transparent"}
            bgColor={"gray.900"}
            _hover={{
               borderColor: "gray",
               borderWidth: 2,
            }}
            borderRadius={8}
            color={"white"}
            width={"550px"}
            alignItems={"flex-start"}
            {...rest}
      >
         <VotingSection
            mt={3}
            px={1}
            userVote={post.userVote!}
            postId={post.id! || ""}
            voteScore={post.voteScore!}
            orientation={"vertical"} />
         <Flex flex={1} p={4} color={"white"} bgColor={"blackAlpha.800"} direction={"column"}>
            <Flex display={"flex"} alignItems={"center"} gap={1}>
               <Avatar
                  boxSize={4}
                  src={post.community?.pictureUrl!}
                  borderRadius={"full"}
                  objectFit={"cover"}
               />
               <Text _hover={{ textDecoration: "underline" }} fontSize={13}>
                  <Link to={`/community/${post.community?.id || ""}`}>
                     r/{post.community?.name!}
                  </Link>
               </Text>
               <span>
                  ·
               </span>
               <Text color={"gray"} fontSize={12}>
                  Posted by <Link to={`/user/${post.author!.id || ""}`}>
                  <Text display={"inline"} _hover={{ textDecoration: "underline" }}>
                     u/{post.author?.userName!}
                  </Text>
               </Link>
                  <Text px={2} display={"inline"}>
                     {moment(Date.parse(post!.createdOn!)).fromNow()}
                  </Text>
               </Text>
            </Flex>
            <Text my={1} textAlign={"left"} fontSize={20}>{post.title!}</Text>
            <Text mb={4} color={"gray.200"} textAlign={"left"} fontSize={16}>{post.content!}</Text>
            {post.mediaUrl && (
               <Flex justifyContent={"center"} alignItems={"center"} width={"full"} mx={-4}>
                  <Image borderRadius={4} boxShadow={"lg"} src={post.mediaUrl} objectFit={"cover"} />
               </Flex>
            )}
            <FeedPostButton
               p={1} my={1} ml={-4}
               alignSelf={"flex-start"}
               Icon={FaRegComment}
               path={`/post/${post.id || ""}`}>
               <Text fontSize={12}>
                  {post.commentsCount} comment{post.commentsCount !== 1 && "s"}
               </Text>
            </FeedPostButton>
         </Flex>
      </Flex>
   );
};

export default FeedPost;