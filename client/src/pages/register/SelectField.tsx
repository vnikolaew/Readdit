import React, { PropsWithChildren } from "react";
import {
   Select as ChakraSelect,
   SelectProps as ChakraSelectProps,
} from "@chakra-ui/react";
import { Field, FieldProps } from "formik";

export interface SelectProps<T> extends PropsWithChildren, ChakraSelectProps {
   onChange?: React.ChangeEventHandler<HTMLSelectElement>;
   options: T[];
   name: string;
   renderOption: (option: T, index: number) => JSX.Element;
}

function Select<T>({ onChange, options, name, renderOption, ...rest }: SelectProps<T>): JSX.Element {
   return (
      <Field name={name}>
         {({ field: { name, value }, form: {setFieldValue}}: FieldProps<string>) => (
            <ChakraSelect value={value} onChange={e => setFieldValue(name, e.target.value)} fontSize={14} {...rest}>
         {options.map(renderOption)}
      </ChakraSelect>
         )}
      </Field>
   );
}

export default Select;
