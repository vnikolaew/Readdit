import React from "react";
import { FormControl, FormErrorMessage, FormLabel, Textarea, TextareaProps } from "@chakra-ui/react";
import { FastField, FieldProps } from "formik";

// @ts-ignore
interface FormFieldProps<T> extends TextareaProps {
   name: keyof T;
   label?: string | null;
}

function FormTextarea<T>({ name, label, ...rest }: FormFieldProps<T>) {
   return (
      <FastField name={name}>
         {({
              field: { name },
              form: { getFieldProps },
              meta: { touched, error },
           }: FieldProps<T>) => (
            <FormControl isInvalid={touched && !!error}>
               {label && <FormLabel fontSize={18}> {label} </FormLabel>}
               <Textarea {...getFieldProps(name)} boxShadow={"md"} {...rest} />
               <FormErrorMessage>{error}</FormErrorMessage>
            </FormControl>
         )}
      </FastField>
   );
};

export default FormTextarea;