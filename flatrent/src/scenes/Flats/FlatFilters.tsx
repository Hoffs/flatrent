import React, { ChangeEvent } from "react";
import Card from "../../components/Card";
import Checkbox from "../../components/Checkbox";
import UserService, { Policies } from "../../services/UserService";
import Styles from "./FlatFilters.module.css";

interface IFlatFilterProps {
  onPageCountChange: (count: number) => void;
  defaultRented?: boolean;
  onShowRentedChange: (showRented: boolean) => void;
}

const getCheckbox = (isEmployee: boolean, defaultValue: boolean, handler: (showRented: boolean) => void) => {
  return isEmployee ? <Checkbox checked={defaultValue} onChange={handler} text="Rodyti išnuomotus" /> : <></>;
};

const FlatFilters = ({ onPageCountChange, defaultRented = false, onShowRentedChange }: IFlatFilterProps) => {
  const handleCountChange = (event: ChangeEvent<HTMLSelectElement>) => onPageCountChange(Number(event.target.value));
  const handleRentedChange = (state: boolean) => onShowRentedChange(state);

  const isEmployee = UserService.hasRoles(...Policies.Employee);
  if (!isEmployee) {
    return <></>;
  }

  return (
    <Card className={Styles.card}>
      {/*
      <select onChange={handleCountChange}>
        <option value={20}>20</option>
        <option value={40}>40</option>
        <option value={80}>80</option>
      </select>
      */}
      {getCheckbox(isEmployee, defaultRented, handleRentedChange)}
    </Card>
  );
};

export default FlatFilters;
