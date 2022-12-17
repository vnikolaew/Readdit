import React, { FC } from "react";
import CommunitySelectDropdown from "./CommunitySelectDropdown";
import { Form, Formik } from "formik";
import { PostApiPostsBody } from "../../api/models";
import { log } from "../../utils/logger";
import { validationSchema } from "./validationSchema";
import { Button, Flex, InputGroup, InputRightElement, Spinner, useToast } from "@chakra-ui/react";
import FormField from "../register/FormField";
import FormTextarea from "../register/FormTextarea";
import ImageSelect from "../register/ImageSelect";
import TagsInput from "./TagsInput";
import { useCreatePostMutation } from "../../api/posts/hooks/useCreatePostMutation";
import { useNavigate } from "react-router-dom";
import { sleep } from "../../utils/sleep";
import ErrorMessage from "../../components/ErrorMessage";
import { ApiError } from "../../api/common/ApiError";

const CreatePostForm: FC = () => {
   const { mutateAsync: createPostAsync, error, isError } = useCreatePostMutation();
   const toast = useToast();
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
               toast({
                  colorScheme: "green",
                  title: "Post created.",
                  description: "Post has been successfully created.",
                  status: "success",
                  duration: 3000,
                  isClosable: true,
               });
               await sleep(1000);

               setSubmitting(false);
               resetForm();

               navigate("/");
            } catch (e) {
               toast({
                  colorScheme: "red",
                  title: "Something went wrong.",
                  description: "Post could not be created.",
                  status: "error",
                  duration: 3000,
                  isClosable: true,
               });
            }
         }
         }>
         {({
              handleSubmit,
              isSubmitting,
              values: { Title, Content },
              errors: { Title: titleError, Content: contentError },
           }) => (
            <Form onSubmit={handleSubmit} style={{ width: "100%" }}>
               <Flex my={10} gap={6} direction={"column"} alignItems={"flex-start"}>
                  <CommunitySelectDropdown />
                  <InputGroup>
                     <FormField<PostApiPostsBody> placeholder={"Title"} name={"Title"} label={"Title"} />
                     <InputRightElement
                        fontWeight={titleError ? "semibold" : "medium"}
                        fontSize={12}
                        width={"50px"}
                        color={titleError ? "red" : "gray"}
                        mr={3}>
                        {Title.length} / 200
                     </InputRightElement>
                  </InputGroup>
                  <InputGroup mb={4}>
                     <FormTextarea<PostApiPostsBody>
                        placeholder={"Content (required)"}
                        name={"Content"}
                        label={"Content"} />
                     <InputRightElement
                        fontWeight={contentError ? "semibold" : "medium"}
                        fontSize={12}
                        width={"50px"}
                        color={contentError ? "red" : "gray"}
                        mr={3}>
                        {Content.length} / 200
                     </InputRightElement>
                  </InputGroup>
                  <TagsInput />
                  <ImageSelect name={"Media"} />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message} />
                  <Button
                     fontSize={20}
                     borderRadius={10}
                     disabled={isSubmitting}
                     spinner={<Spinner colorScheme={"white"} size={"md"} />}
                     px={12}
                     py={6}
                     alignSelf={"flex-end"}
                     boxShadow={"lg"}
                     colorScheme={"facebook"}
                     variant={"solid"}
                     type={"submit"}>Post</Button>
               </Flex>
            </Form>
         )}
      </Formik>
   );
};

export default CreatePostForm;