import React, { FC } from "react";
import { Box, Button, FormControl, FormLabel, Heading, Spinner, Text } from "@chakra-ui/react";
import { useRegisterMutation } from "../../api/identity";
import { Link, useNavigate } from "react-router-dom";
import { Form, Formik } from "formik";
import { PostApiIdentityRegisterBody } from "../../api/models";
import { useGetAllCountriesQuery } from "../../api/countries/hooks/useGetAllCountriesQuery";
import FormField from "./FormField";
import { allGenders, registerSchema } from "./validationSchema";
import SelectField from "./SelectField";
import ImageSelect from "./ImageSelect";
import { WarningIcon } from "@chakra-ui/icons";
import { ApiError } from "../../api/common/ApiError";
import { log } from "../../utils/logger";

const Register: FC = () => {
   const { mutateAsync, isError, error } = useRegisterMutation();
   const { data: countries } = useGetAllCountriesQuery();
   const navigate = useNavigate();

   return (
      <Box mt={10} mx={"auto"} width={"350px"}>
         <Heading textAlign={"left"} size={"lg"} mb={6}>
            Register
         </Heading>
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
                  <FormControl>
                     <FormLabel>Country: </FormLabel>
                     <SelectField
                        name={"Country"}
                        options={(countries || []).sort((a, b) =>
                           a.name!.localeCompare(b.name!, "en", {
                              sensitivity: "base",
                           }),
                        )}
                        renderOption={(c) => (
                           <option value={c.name!} key={c.code}>
                              {c.name}
                           </option>
                        )}
                     />
                  </FormControl>
                  <FormControl>
                     <FormLabel>Select a Gender: </FormLabel>
                     <SelectField
                        name={"Gender"}
                        options={allGenders}
                        renderOption={(g, index) => (
                           <option value={g!} key={index}>
                              {g}
                           </option>
                        )}
                     />
                  </FormControl>
                  <FormControl>
                     <FormLabel>Select a profile image: </FormLabel>
                     <ImageSelect name={"ProfilePicture"} />
                  </FormControl>
                  <FormField<PostApiIdentityRegisterBody>
                     name={"Password"}
                     label={"Password: "}
                     type={"password"}
                     placeholder={"Type your password ..."}
                  />
                  {isError && (
                     <Text textAlign={"start"} color={"red"}>
                        <span style={{ marginRight: ".5rem" }}><WarningIcon color={"red"} fontSize={14} /></span>
                        {(error as ApiError).errors[0]}
                     </Text>
                  )
                  }
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
                     Register
                  </Button>
                  <Text fontSize="1.1rem" textAlign={"center"}>
                     Already have an account?{" "}
                     <Box
                        fontWeight={"medium"}
                        fontSize="1.1rem"
                        ml={2}
                        display={"inline"}
                        color={"messenger.500"}
                     >
                        <Link to={"/login"}>Login here!</Link>
                     </Box>
                  </Text>
               </Form>
            )}
         </Formik>
      </Box>
   );
};

export default Register;
