import { Dom } from "OneJS/Dom";
import { h } from "preact";
import { useEffect, useRef } from "preact/hooks";
import { Style } from "preact/jsx";

interface Props {
  style: Style;
  text: string;
  enabled: boolean;
  onClick?: Function;
}

export const Button = ({ style, text, enabled, onClick }: Props) => {
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
        ...style,
        borderColor: "#FFFFFF",
        backgroundColor: "#4E7CFF",
        color: enabled ? "#FFFFFF" : "#A9A9A9",
        fontSize: "16px",
      }}
      onClick={() => onClick()}
    />
  );
};
