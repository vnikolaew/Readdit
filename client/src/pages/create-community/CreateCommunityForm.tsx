import { Form, Formik } from "formik";
import React, { FC } from "react";
import { useNavigate } from "react-router-dom";
import { PostApiCommunitiesBody } from "../../api/models";
import { useCreateCommunityMutation } from "../../api/communities";
import { log } from "../../utils/logger";
import FormField from "../register/FormField";
import FormTextarea from "../register/FormTextarea";
import ImageSelect from "../register/ImageSelect";
import { ApiError } from "../../api/common/ApiError";
import TagsInput from "../create-post/TagsInput";
import ErrorMessage from "../../components/ErrorMessage";
import { validationSchema } from "./validationSchema";
import CommunityTypeSelector from "./CommunityTypeSelector";
import { Button, Flex, Loader, Text, Title, useMantineTheme } from "@mantine/core";
import { showNotification } from "@mantine/notifications";

const CreateCommunityForm: FC = () => {
   const { mutateAsync: createCommunityAsync, error, isError } = useCreateCommunityMutation();
   const theme = useMantineTheme();
   const navigate = useNavigate();

   return (
      <Formik<PostApiCommunitiesBody>
         initialValues={{
            Name: ""!,
            Description: "",
            Picture: null!,
            Tags: [],
            Type: 1,
         }}
         validationSchema={validationSchema}
         onSubmit={async (values, { setSubmitting, resetForm }) => {
            log(values);
            try {
               const response = await createCommunityAsync(values);
               showNotification({
                  title: "Community created.",
                  message: `Community ${values.Name} has been successfully created.`,
                  color: theme.colors.green[3],
                  autoClose: 3000,
                  disallowClose: false,
               });

               setSubmitting(false);
               resetForm();

               navigate("/");
            } catch (e) {
               showNotification({
                  color: theme.colors.red[3],
                  autoClose: 3000,
                  disallowClose: false,
                  title: "Something went wrong.",
                  message: "Community could not be created.",
               });
            }
         }}
      >
         {({
            handleSubmit,
            isSubmitting,
            values: { Name, Description },
            errors: { Name: nameError, Description: descriptionError },
         }) => (
            <Form onSubmit={handleSubmit} style={{ width: "100%" }}>
               <Flex my={10} gap={36} direction={"column"} align={"flex-start"}>
                  <FormField<PostApiCommunitiesBody>
                     placeholder={"Title"}
                     name={"Name"}
                     w={"80%"}
                     rightSectionWidth={60}
                     rightSection={
                        <Text
                           fw={nameError ? "bold" : "normal"}
                           fz={12}
                           w={"50px"}
                           color={nameError ? theme.colors.red[6] : theme.colors.gray[3]}
                           mr={3}
                        >
                           {Name.length} / 200
                        </Text>
                     }
                     label={"Name"}
                  />
                  <FormTextarea<PostApiCommunitiesBody>
                     placeholder={"Content (required)"}
                     name={"Description"}
                     rightSectionWidth={60}
                     autosize
                     w={"80%"}
                     rightSection={
                        <Text
                           fw={descriptionError ? "bold" : "normal"}
                           my={14}
                           sx={{
                              alignSelf: "flex-start",
                           }}
                           fz={12}
                           w={"50px"}
                           color={descriptionError ? theme.colors.red[6] : theme.colors.gray[6]}
                           mr={3}
                        >
                           {Description.length} / 200
                        </Text>
                     }
                     label={"Description"}
                  />
                  <TagsInput />
                  <CommunityTypeSelector />
                  <Title mt={2} fw={"normal"} fz={20}>
                     Select an image:
                  </Title>
                  <ImageSelect name={"Picture"} />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message} />
                  <Button
                     fz={20}
                     radius={"xl"}
                     disabled={isSubmitting}
                     loading={isSubmitting}
                     loaderProps={<Loader color={theme.colors.gray[0]} size={"md"} />}
                     px={50}
                     size={"md"}
                     sx={{
                        alignSelf: "flex-end",
                        boxShadow: theme.shadows.md,
                     }}
                     bg={theme.colors.gray[1]}
                     color={theme.colors.blue[6]}
                     variant={"default"}
                     type={"submit"}
                  >
                     {!isSubmitting && "Create"}
                  </Button>
               </Flex>
            </Form>
         )}
      </Formik>
   );
};

export default CreateCommunityForm;
