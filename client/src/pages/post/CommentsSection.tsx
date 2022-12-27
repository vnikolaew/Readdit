import React, { FC } from "react";
import { useCreateCommentMutation } from "../../api/comments/hooks/useCreateCommentMutation";
import Link from "../../components/Link";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { Form, Formik } from "formik";
import { PostApiCommentsBody, PostCommentDetailsModel } from "../../api/models";
import { log } from "../../utils/logger";
import FormTextarea from "../register/FormTextarea";
import PostComment from "./PostComment";
import { Button, Flex, Loader, Stack, Text, useMantineTheme } from "@mantine/core";

interface IProps {
   postId: string;
   comments: PostCommentDetailsModel[];
}

const CommentsSection: FC<IProps> = ({ postId, comments }) => {
   const user = useCurrentUser();
   const theme = useMantineTheme();
   const { data: userDetails } = useGetUserDetailsQuery(user?.userId);
   const { mutateAsync: commentAsync } = useCreateCommentMutation();

   return (
      <Flex
         sx={(theme) => ({
            borderWidth: 1,
            borderColor: "transparent",
            borderRadius: 8,
            "&:hover": {
               borderColor: theme.colors.gray[6],
               borderWidth: 1,
            },
         })}
         bg={theme.colors.gray[9]}
         p={24}
         color={theme.colors.dark[0]}
         direction={"column"}
         w={"600px"}
      >
         <Text color={theme.colors.gray[0]} style={{ alignSelf: "flex-start" }} mb={2} fz={14}>
            Comment as
            <Link to={`/user/${user?.userId}`}>
               <Text fz={14} color={theme.colors.blue[4]} pl={8} span>
                  {userDetails!.userName!}
               </Text>
            </Link>
         </Text>
         <Formik<PostApiCommentsBody>
            initialValues={{ PostId: postId, Content: "" }}
            onSubmit={async (values, { setSubmitting, resetForm }) => {
               log(values);
               const response = await commentAsync(values);
               log(response);

               setSubmitting(false);
               resetForm();
            }}
         >
            {({ isSubmitting, values: { Content } }) => (
               <Form>
                  <Flex direction={"column"}>
                     <FormTextarea<PostApiCommentsBody>
                        placeholder={"What are your thoughts?"}
                        fz={20}
                        styles={(theme) => ({
                           input: {
                              backgroundColor: theme.colors.gray[9],
                              color: theme.colors.gray[0],
                              fontsize: 20,
                           },
                           root: {
                              backgroundColor: theme.colors.gray[9],
                              width: "100%",
                              fontsize: 20,
                           },
                        })}
                        name={"Content"}
                     />
                     <Button
                        mt={10}
                        px={32}
                        size={"xs"}
                        variant={"default"}
                        fz={12}
                        color={theme.colors.dark[9]}
                        styles={(theme) => ({
                           root: {
                              alignSelf: "flex-end",
                              "&:hover": {
                                 backgroundColor: theme.colors.gray[2],
                                 color: theme.colors.dark[9],
                              },
                              cursor: !Content ? "not-allowed" : "pointer",
                           },
                        })}
                        radius={"lg"}
                        bg={theme.colors.gray[0]}
                        loading={isSubmitting}
                        disabled={isSubmitting || !Content}
                        loaderProps={<Loader color={theme.colors.gray[0]} size={"md"} />}
                        type={"submit"}
                     >
                        Comment
                     </Button>
                  </Flex>
               </Form>
            )}
         </Formik>
         <Stack mt={4} spacing={4}>
            {comments.map((comment, id) => (
               <PostComment postId={postId} key={id} comment={comment} />
            ))}
         </Stack>
      </Flex>
   );
};

export default CommentsSection;
