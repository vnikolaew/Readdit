import React, { FC } from "react";
import { useRegisterMutation } from "../../api/identity";
import { useNavigate } from "react-router-dom";
import { Form, Formik } from "formik";
import { PostApiIdentityRegisterBody } from "../../api/models";
import { useGetAllCountriesQuery } from "../../api/countries/hooks/useGetAllCountriesQuery";
import FormField from "./FormField";
import { allGenders, registerSchema } from "./validationSchema";
import SelectField from "./SelectField";
import ImageSelect from "./ImageSelect";
import { ApiError } from "../../api/common/ApiError";
import { log } from "../../utils/logger";
import ErrorMessage from "../../components/ErrorMessage";
import { Anchor, Box, Button, Container, Input, Loader, Text, Title } from "@mantine/core";
import Link from "../../components/Link";

const Register: FC = () => {
   const { mutateAsync, isError, error } = useRegisterMutation();
   const { data: countries } = useGetAllCountriesQuery();
   const navigate = useNavigate();

   return (
      <Box mt={10} mx={"auto"} w={"350px"}>
         <Title order={1} style={{ textAlign: "start" }} mb={24}>
            Register
         </Title>
         <Formik<PostApiIdentityRegisterBody>
            validationSchema={registerSchema}
            initialValues={{
               Username: "",
               Password: "",
               Country: "",
               Email: "",
               EmailConfirmationUrl: "<SOME_URL>",
               FirstName: "",
               LastName: "",
               Gender: "",
               ProfilePicture: null!,
            }}
            onSubmit={async (values, { setSubmitting }) => {
               log(values);

               const formData = new FormData();
               Object.entries(values).forEach(([key, value]) => {
                  if (value instanceof File) {
                     formData.append(key, value, value.name);
                  } else {
                     formData.append(key, value);
                  }
               });

               const response = await mutateAsync(formData);

               setSubmitting(false);
               if (!!(response as any).token) {
                  navigate("/");
               }
            }}
         >
            {({ isSubmitting }) => (
               <Form
                  style={{
                     display: "flex",
                     gap: "2rem",
                     flexDirection: "column",
                  }}
               >
                  <FormField<PostApiIdentityRegisterBody>
                     name={"FirstName"}
                     label={"First Name: "}
                     placeholder={"First name ..."}
                  />
                  <FormField<PostApiIdentityRegisterBody>
                     name={"LastName"}
                     label={"Last Name: "}
                     placeholder={"Last name ..."}
                  />
                  <FormField<PostApiIdentityRegisterBody>
                     name={"Email"}
                     label={"Email address: "}
                     placeholder={"foo@bar.com"}
                  />
                  <FormField<PostApiIdentityRegisterBody>
                     name={"Username"}
                     label={"Username: "}
                     placeholder={"Select a cool username ..."}
                  />
                  <Input.Wrapper
                     sx={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                        gap: 2,
                        alignItems: "flex-start",
                     }}
                  >
                     <Input.Label size={"lg"}>Country: </Input.Label>
                     <SelectField
                        w={"100%"}
                        name={"Country"}
                        options={(countries || []).sort((a, b) =>
                           a.name!.localeCompare(b.name!, "en", {
                              sensitivity: "base",
                           })
                        )}
                        getName={(c) => c.name!}
                     />
                  </Input.Wrapper>
                  <Input.Wrapper
                     sx={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                        gap: 2,
                        alignItems: "flex-start",
                     }}
                  >
                     <Input.Label size={"lg"}>Select a Gender:</Input.Label>
                     <SelectField w={"100%"} name={"Gender"} options={allGenders} />
                  </Input.Wrapper>
                  <Input.Wrapper
                     sx={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                        gap: 2,
                        alignItems: "flex-start",
                     }}
                  >
                     <Input.Label size={"md"}>Select a profile image: </Input.Label>
                     <ImageSelect name={"ProfilePicture"} />
                  </Input.Wrapper>
                  <FormField<PostApiIdentityRegisterBody>
                     name={"Password"}
                     autoComplete={"autoComplete"}
                     label={"Password: "}
                     type={"password"}
                     placeholder={"Type your password ..."}
                  />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message ?? ""} />
                  <Button
                     type={"submit"}
                     style={{ alignSelf: "flex-end" }}
                     py={5}
                     size={"md"}
                     w={"60%"}
                     loaderProps={<Loader color={"blue"} size={"lg"} />}
                     loading={isSubmitting}
                     loaderPosition={"center"}
                     mt={4}
                     color={"blue"}
                  >
                     {!isSubmitting && "Register"}
                  </Button>
                  <Text fw={"normal"} size={"sm"} align={"end"}>
                     Already have an account?{" "}
                     <Container size={"sm"} ml={10} px={0} display={"inline"}>
                        <Anchor
                           fw={"bold"}
                           size={"sm"}
                           styles={(theme) => ({
                              "&.hover": {
                                 color: theme.colors.blue[5],
                              },
                           })}
                        >
                           <Link to={"/login"}>Login here!</Link>
                        </Anchor>
                     </Container>
                  </Text>
               </Form>
            )}
         </Formik>
      </Box>
   );
};

export default Register;
