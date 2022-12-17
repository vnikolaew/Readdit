import React, { FC } from "react";
import {
   Box,
   Button,
   Heading,
   Spinner,
   Text,
} from "@chakra-ui/react";
import { Form, Formik } from "formik";
import { PostApiIdentityLoginBody } from "../../api/models";
import { useLoginMutation } from "../../api/identity";
import { loginSchema } from "./validationSchema";
import { Link, useNavigate } from "react-router-dom";
import FormField from "../register/FormField";
import { ApiError } from "../../api/common/ApiError";
import { log } from "../../utils/logger";
import ErrorMessage from "../../components/ErrorMessage";

interface LoginProps {}

const Login: FC<LoginProps> = () => {
   const { mutateAsync, isError, error } = useLoginMutation();
   const navigate = useNavigate();

   return (
      <Box mt={10} mx={"auto"} width={"300px"}>
         <Heading textAlign={"left"} size={"lg"} mb={6}>
            Login
         </Heading>
         <Formik<PostApiIdentityLoginBody>
            validationSchema={loginSchema}
            initialValues={{ Username: "", Password: "" }}
            onSubmit={async (values, { setSubmitting }) => {
               log(values);
               const response = await mutateAsync(values);

               setSubmitting(false)
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
                     boxShadow={"md"}
                     placeholder={"Username"}
                     name={"Username"} label={"Username: "} />
                  <FormField<PostApiIdentityLoginBody>
                     boxShadow={"md"}
                     type={'password'}
                     placeholder={"Type your password ..."}
                     name={"Password"} label={"Password: "} />
                  <ErrorMessage show={isError} errorMessage={(error as ApiError)?.message} />
                  <Button
                     type={"submit"}
                     alignSelf={"flex-end"}
                     py={5}
                     fontSize={16}
                     width={"60%"}
                     spinner={<Spinner color={"white"} size={"md"} />}
                     isLoading={isSubmitting}
                     mt={4}
                     colorScheme={"twitter"}
                  >
                     Login
                  </Button>
                  <Text fontSize="1rem" textAlign={"center"}>
                     Don't have an account yet?
                     <Box
                        fontWeight={"medium"}
                        ml={2}
                        fontSize="1.1rem"
                        display={"inline"}
                        color={"messenger.500"}
                     >
                        <Link to={"/register"}>Sign up now!</Link>
                     </Box>
                  </Text>
               </Form>
            )}
         </Formik>
      </Box>
   );
};

export default Login;
