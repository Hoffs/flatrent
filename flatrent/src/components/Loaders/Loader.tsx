import React, { Children, ReactNode } from "react";
import { ScaleLoader } from "react-spinners";
import Styles from "./Loader.module.css";

interface ILoaderProps {
  loading: boolean;
  children: ReactNode[];
}

const Loader = ({loading = true, children}: ILoaderProps) => {
  const showing: ReactNode = loading ? (<ScaleLoader color={"#ff5a5e"} loading={loading}/>) : children;
  console.log(children)
  return (<div className={Styles.wrapper}>
      {showing}
      {/* <ScaleLoader color={"#ff5a5e"} loading={loading}/> */}
      {/* {loading ? (<></>) : {...children}} */}
    </div>);
};


// const Loader = (props: ILoaderProps) => {
//   return props.loading ? <ScaleLoader color={"#ff5a5e"} loading={props.loading}/> : props.children;
// };

export default Loader;
