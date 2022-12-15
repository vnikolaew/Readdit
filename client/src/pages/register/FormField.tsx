import React from "react";
import { FastField, FieldProps } from "formik";
import { FormControl, FormErrorMessage, FormLabel, Input, InputProps } from "@chakra-ui/react";

// @ts-ignore
interface FormFieldProps<T> extends InputProps {
   name: keyof T;
   label?: string | null;
}

function FormField<T>({ name, label, ...rest }: FormFieldProps<T>) {
   return (
      <FastField name={name}>
         {({
              field: { name },
              form: { getFieldProps },
              meta: { touched, error },
           }: FieldProps<T>) => (
            <FormControl isInvalid={touched && !!error}>
               {label && <FormLabel fontSize={18}> {label} </FormLabel>}
               <Input {...getFieldProps(name)} boxShadow={"md"} {...rest} />
               <FormErrorMessage>{error}</FormErrorMessage>
            </FormControl>
         )}
      </FastField>
   );
};

export default FormField;