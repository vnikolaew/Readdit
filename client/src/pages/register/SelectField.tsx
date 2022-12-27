import React, { PropsWithChildren } from "react";
import {
   Select as MantineSelect,
   SelectProps as MSelectProps,
} from "@mantine/core";
import { Field, FieldProps } from "formik";

export interface SelectProps<T>
   extends PropsWithChildren,
      Partial<MSelectProps> {
   options: T[];
   getName?: (item: T) => string;
   name: string;
}

function Select<T>({
   options,
   name,
   getName = (x) => x as string,
   ...rest
}: SelectProps<T>): JSX.Element {
   // @ts-ignore
   return (
      <Field name={name}>
         {({
            field: { name, value },
            form: { setFieldValue },
         }: FieldProps<string>) => (
            <MantineSelect
               data={options.map((o) => ({
                  value: getName(o),
                  label: getName(o),
               }))}
               searchable
               size={"sm"}
               value={value as string}
               onChange={(value) => setFieldValue(name, value)}
               {...rest}
            />
         )}
      </Field>
   );
}

export default Select;
