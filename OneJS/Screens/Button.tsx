import { Dom } from "OneJS/Dom";
import { h } from "preact";
import { useEffect, useRef } from "preact/hooks";

interface Props {
  text: string;
  enabled: boolean;
  onClick?: Function;
}

export const Button = ({ text, enabled, onClick }: Props) => {
  const buttonRef = useRef<Dom>();

  useEffect(() => {
    buttonRef.current.ve.SetEnabled(enabled);
  }, [enabled]);

  return (
    <button
      ref={buttonRef}
      name="ok"
      text={text}
      style={{
        borderColor: "#FFFFFF",
        backgroundColor: "#4E7CFF",
        color: enabled ? "#FFFFFF" : "#A9A9A9",
        fontSize: "12px",
        borderRadius: 4,
      }}
      onClick={() => onClick()}
    />
  );
};
