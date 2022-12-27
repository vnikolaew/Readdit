import React, { FC } from "react";
import { Form, Formik } from "formik";
import { PostApiIdentityLoginBody } from "../../api/models";
import { useLoginMutation } from "../../api/identity";
import { loginSchema } from "./validationSchema";
import { useNavigate } from "react-router-dom";
import FormField from "../register/FormField";
import { ApiError } from "../../api/common/ApiError";
import { log } from "../../utils/logger";
import ErrorMessage from "../../components/ErrorMessage";
import { Anchor, Box, Button, Container, Loader, Text, Title } from "@mantine/core";
import Link from "../../components/Link";

interface LoginProps {}

const Login: FC<LoginProps> = () => {
   const { mutateAsync, isError, error } = useLoginMutation();
   const navigate = useNavigate();

   return (
      <Box w={300} mt={10} mx={"auto"}>
         <Title order={2} mb={6}>
            Login
         </Title>
         <Formik<PostApiIdentityLoginBody>
            validationSchema={loginSchema}
            initialValues={{ Username: "", Password: "" }}
            onSubmit={async (values, { setSubmitting }) => {
               log(values);
               const response = await mutateAsync(values);

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
                  <FormField<PostApiIdentityLoginBody>
                     placeholder={"Username"}
                     name={"Username"}
                     label={"Username: "}
                  />
                  <FormField<PostApiIdentityLoginBody>
                     type={"password"}
                     autoComplete={"autocomplete"}
                     placeholder={"Type your password ..."}
                     name={"Password"}
                     label={"Password: "}
                  />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message} />
                  <Button
                     type={"submit"}
                     style={{ alignSelf: "flex-end" }}
                     py={5}
                     variant={"filled"}
                     w={"60%"}
                     loading={isSubmitting}
                     loaderProps={<Loader color={"blue"} size={"lg"} />}
                     loaderPosition={"center"}
                     mt={4}
                  >
                     {!isSubmitting && "Login"}
                  </Button>
                  <Text fw={"normal"} size={"sm"} align={"end"}>
                     Don't have an account yet?
                     <Container size={"sm"} px={0} ml={12} display={"inline"}>
                        <Anchor
                           fw={"bold"}
                           size={"sm"}
                           styles={(theme) => ({
                              "&.hover": {
                                 color: theme.colors.blue[5],
                              },
                           })}
                        >
                           <Link to={"/register"}>Sign up now!</Link>
                        </Anchor>
                     </Container>
                  </Text>
               </Form>
            )}
         </Formik>
      </Box>
   );
};

export default Login;
