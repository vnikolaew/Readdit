import React, { FC } from "react";
import { Button, Flex, Spinner, Text, VStack } from "@chakra-ui/react";
import { useCreateCommentMutation } from "../../api/comments/hooks/useCreateCommentMutation";
import { Link } from "react-router-dom";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { Form, Formik } from "formik";
import { PostApiCommentsBody, PostCommentDetailsModel } from "../../api/models";
import { log } from "../../utils/logger";
import FormTextarea from "../register/FormTextarea";
import PostComment from "./PostComment";

interface IProps {
   postId: string;
   comments: PostCommentDetailsModel[];
}

const CommentsSection: FC<IProps> = ({ postId, comments }) => {
   const user = useCurrentUser();
   const { data: userDetails } = useGetUserDetailsQuery(user?.userId);
   const { mutateAsync: commentAsync } = useCreateCommentMutation();

   return (
      <Flex
         bgColor={"gray.900"}
         borderRadius={6}
         boxShadow={"lg"}
         p={6}
         color={"white"}
         direction={"column"} width={"600px"}>
         <Text mb={2} alignSelf={"flex-start"} fontSize={14}>Comment as
            <Text color={"blue.500"}
                  pl={1}
                  display={"inline"}
                  _hover={{ textDecoration: "underline" }}>
               <Link to={`/user/${user?.userId!}`}>
                  {userDetails!.userName!}
               </Link>
            </Text>
         </Text>
         <Formik<PostApiCommentsBody>
            initialValues={{ PostId: postId, Content: "" }}
            onSubmit={async (values, { setSubmitting, resetForm }) => {
               log(values);
               const response = await commentAsync(values);
               log(response);

               setSubmitting(false);
               resetForm();
            }}>
            {({ isSubmitting, values }) => (
               <Form>
                  <Flex direction={"column"}>
                     <FormTextarea<PostApiCommentsBody>
                        placeholder={"What are your thoughts?"}
                        name={"Content"} />
                     <Button mt={2} px={6} size={"sm"} fontSize={12} color={"black"} borderRadius={"full"}
                             bgColor={"white"}
                             disabled={isSubmitting}
                             spinner={<Spinner color={"white"} size={"md"} />}
                             alignSelf={"flex-end"}
                             type={"submit"}>Comment</Button>
                  </Flex>
               </Form>
            )}
         </Formik>
         <VStack mt={4} spacing={4}>
            {comments.map((comment, id) => (
               <PostComment postId={postId} key={id} comment={comment} />
            ))}
         </VStack>
      </Flex>
   );
};

export default CommentsSection;