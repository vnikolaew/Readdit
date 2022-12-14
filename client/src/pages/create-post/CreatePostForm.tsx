import React, { FC } from "react";
import CommunitySelectDropdown from "./CommunitySelectDropdown";
import { Form, Formik } from "formik";
import { PostApiPostsBody } from "../../api/models";
import { log } from "../../utils/logger";
import { validationSchema } from "./validationSchema";
import FormField from "../register/FormField";
import FormTextarea from "../register/FormTextarea";
import ImageSelect from "../register/ImageSelect";
import TagsInput from "./TagsInput";
import { useCreatePostMutation } from "../../api/posts/hooks/useCreatePostMutation";
import { useNavigate } from "react-router-dom";
import { sleep } from "../../utils/sleep";
import ErrorMessage from "../../components/ErrorMessage";
import { ApiError } from "../../api/common/ApiError";
import { Button, Flex, Loader, Text, Title as MTitle, useMantineTheme } from "@mantine/core";
import { showNotification } from "@mantine/notifications";

const CreatePostForm: FC = () => {
   const { mutateAsync: createPostAsync, error, isError } = useCreatePostMutation();
   const theme = useMantineTheme();
   const navigate = useNavigate();

   return (
      <Formik<PostApiPostsBody>
         initialValues={{
            CommunityId: null!,
            Content: "",
            Media: null!,
            Tags: [],
            Title: "",
         }}
         validationSchema={validationSchema}
         onSubmit={async (values, { setSubmitting, resetForm }) => {
            log(values);
            try {
               const response = await createPostAsync(values);
               showNotification({
                  title: "Post created.",
                  message: "Post has been successfully created.",
                  color: theme.colors.green[3],
                  autoClose: 3000,
                  disallowClose: false,
               });
               await sleep(1000);

               setSubmitting(false);
               resetForm();

               navigate("/");
            } catch (e) {
               showNotification({
                  title: "Something went wrong.",
                  message: "Post could not be created.",
                  color: theme.colors.red[3],
                  autoClose: 3000,
                  disallowClose: false,
               });
            }
         }}
      >
         {({
            handleSubmit,
            isSubmitting,
            values: { Title, Content },
            errors: { Title: titleError, Content: contentError },
         }) => (
            <Form onSubmit={handleSubmit} style={{ width: "100%" }}>
               <Flex w={"100%"} my={10} gap={24} direction={"column"} align={"flex-start"}>
                  <CommunitySelectDropdown />
                  <FormField<PostApiPostsBody>
                     w={"100%"}
                     rightSectionWidth={80}
                     rightSection={
                        <Text
                           sx={{
                              alignSelf: "flex-start",
                           }}
                           fw={titleError ? "semibold" : "medium"}
                           fz={12}
                           my={8}
                           color={titleError ? "red" : "gray"}
                           mr={3}
                        >
                           {Title.length} / 200
                        </Text>
                     }
                     placeholder={"Title"}
                     name={"Title"}
                     label={"Title"}
                  />
                  <FormTextarea<PostApiPostsBody>
                     w={"100%"}
                     rightSectionWidth={80}
                     autosize
                     rightSection={
                        <Text
                           sx={{
                              alignSelf: "flex-start",
                           }}
                           fw={contentError ? "semibold" : "medium"}
                           fz={12}
                           color={contentError ? "red" : "gray"}
                           my={14}
                           mr={8}
                        >
                           {Content.length} / 200
                        </Text>
                     }
                     placeholder={"Content (required)"}
                     name={"Content"}
                     label={"Content"}
                  />
                  <TagsInput />
                  <MTitle key={1} mt={2} fw={"medium"} fz={20}>
                     Select an image:
                  </MTitle>
                  <ImageSelect name={"Media"} />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message} />
                  <Button
                     fz={20}
                     radius={"xl"}
                     disabled={isSubmitting}
                     loading={isSubmitting}
                     loaderProps={<Loader color={theme.colors.gray[0]} size={"md"} />}
                     px={50}
                     size={"md"}
                     // py={8}
                     sx={{
                        alignSelf: "flex-end",
                        boxShadow: theme.shadows.md,
                     }}
                     bg={theme.colors.gray[1]}
                     color={theme.colors.blue[6]}
                     variant={"default"}
                     type={"submit"}
                  >
                     {!isSubmitting && "Post"}
                  </Button>
               </Flex>
            </Form>
         )}
      </Formik>
   );
};

export default CreatePostForm;
