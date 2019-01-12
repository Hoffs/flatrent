import React, { ChangeEvent } from "react";
import Card from "../../components/Card";
import Checkbox from "../../components/Checkbox";
import Styles from "./FlatFilters.module.css";

interface IFlatFilterProps {
  onPageCountChange: (count: number) => void;
  onShowRentedChange: (showRented: boolean) => void;
}

const FlatFilters = (props: IFlatFilterProps) => {
  const handleCountChange = (event: ChangeEvent<HTMLSelectElement>) =>
    props.onPageCountChange(Number(event.target.value));
  const handleRentedChange = (state: boolean) =>
    props.onShowRentedChange(state);

  return (
    <Card className={Styles.card}>
      <select onChange={handleCountChange}>
        <option value={20}>20</option>
        <option value={40}>40</option>
        <option value={80}>80</option>
      </select>
      <Checkbox onChange={handleRentedChange} text="Išnuomotus" />
    </Card>
  );
};

export default FlatFilters;
