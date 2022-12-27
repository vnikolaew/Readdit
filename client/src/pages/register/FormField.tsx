import React from "react";
import { FastField, FieldProps } from "formik";
import { Input, TextInput, TextInputProps, Title } from "@mantine/core";

// @ts-ignore
interface FormFieldProps<T> extends TextInputProps {
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
            <Input.Wrapper
               display={"flex"}
               w={rest.w}
               style={{ alignItems: "flex-start", gap: "10px", flexDirection: "column" }}
               label={<Title fw={"normal"} size={"h4"}>{label}</Title>}
               error={touched && !!error && error}>
               <TextInput
                  w={"100%"}
                  {...getFieldProps(name)} {...rest} />
            </Input.Wrapper>
         )}
      </FastField>
   );
};

export default FormField;